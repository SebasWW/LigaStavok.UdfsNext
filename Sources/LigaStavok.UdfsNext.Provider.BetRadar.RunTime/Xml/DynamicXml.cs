using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Xml
{
    public class DynamicXml : DynamicObject
    {
        private readonly XElement _root;

        private DynamicXml(XElement root)
        {
            _root = root;
        }

        private static object ChangeType(string value, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return ChangeType(value, underlyingType);
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, value);
            }

            if (targetType == typeof(double))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(value);
            }

            return Convert.ChangeType(value, targetType);
        }

        private static object CreateResult(XElement element)
        {
            return element.HasElements || element.HasAttributes || element.IsEmpty
                 ? (object)new DynamicXml(element)
                 : element.Value;
        }

        public static DynamicXml Load(string filename)
        {
            return new DynamicXml(XDocument.Load(filename).Root);
        }

        private static string NormalizeName(XName name)
        {
            return NormalizeName(name.LocalName);
        }

        private static string NormalizeName(string name)
        {
            return name.Replace(".", string.Empty)
                       .Replace("_", string.Empty)
                       .Replace("-", string.Empty)
                       .ToLowerInvariant();
        }

        public static DynamicXml Parse(string xmlString)
        {
            return new DynamicXml(XDocument.Parse(xmlString).Root);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var name = NormalizeName(binder.Name);

            var attribute = _root.Attributes().SingleOrDefault(x => NormalizeName(x.Name) == name);
            if (attribute != null)
            {
                result = attribute.Value;

                return true;
            }

            var element = _root.Elements().SingleOrDefault(x => NormalizeName(x.Name) == name);
            if (element != null)
            {
                result = CreateResult(element);

                return true;
            }

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            switch (binder.Name)
            {
                case nameof(GetName):
                case nameof(GetValue):
                    {
                        return base.TryInvokeMember(binder, args, out result);
                    }
                default:
                    {
                        var name = NormalizeName(binder.Name);

                        var match = Regex.Match(name, @"get([a-z0-9]+)list");
                        if (match.Success)
                        {
                            var childElementName = NormalizeName(match.Groups[1].Value);

                            result = _root.Elements()
                                .Where(x => NormalizeName(x.Name) == childElementName)
                                .Select(CreateResult)
                                //.ToImmutableArray();
                                .ToArray();

                            return true;
                        }

                        var attribute = _root.Attributes().SingleOrDefault(x => NormalizeName(x.Name) == name);
                        if (attribute != null)
                        {
                            var csharpBinder = binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");

                            if (csharpBinder.GetProperty("TypeArguments")?.GetValue(binder, null) is IList<Type> typeArgs && typeArgs.Count > 0)
                            {
                                result = ChangeType(attribute.Value, typeArgs[0]);
                            }
                            else
                            {
                                throw new InvalidOperationException("Output type is not specified, use property getter to get raw value.");
                            }

                            return true;
                        }

                        result = null;

                        return true;
                    }
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return false;
        }

        public IDictionary<string, string> GetAttributes()
        {
            return _root.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value);
        }

        public string GetName()
        {
            return _root.Name.LocalName;
        }

        public string GetValue()
        {
            return _root.Value;
        }

        public object GetValue<T>()
        {
            return ChangeType(_root.Value, typeof(T));
        }
    }
}