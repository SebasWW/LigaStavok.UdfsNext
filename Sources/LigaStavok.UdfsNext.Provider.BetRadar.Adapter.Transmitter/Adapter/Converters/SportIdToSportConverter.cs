using Udfs.Transmitter.Messages.Identifiers;

namespace LigaStavok.UdfsNext.Provider.BetRadar.Adapter.Converters
{
    public static class SportIdToSportConverter
    {
        public static Sport Convert(string sportId)
        {
            switch (sportId)
            {
                case "sr:sport:1":   return Sport.Football;
                case "sr:sport:2":   return Sport.Basketball;
                case "sr:sport:3":   return Sport.Baseball;
                case "sr:sport:4":   return Sport.IceHockey;
                case "sr:sport:5":   return Sport.Tennis;
                case "sr:sport:6":   return Sport.Handball;
                case "sr:sport:7":   return Sport.Floorball;
                case "sr:sport:8":   return Sport.Trotting;
                case "sr:sport:9":   return Sport.Golf;
                case "sr:sport:10":  return Sport.Boxing;
                case "sr:sport:11":  return Sport.MotorSport;
                case "sr:sport:12":  return Sport.Rugby;
                case "sr:sport:13":  return Sport.AustralianFootball;
                case "sr:sport:14":  return Sport.WinterSports;
                case "sr:sport:15":  return Sport.Bandy;
                case "sr:sport:16":  return Sport.AmericanFootball;
                case "sr:sport:17":  return Sport.Cycling;
                case "sr:sport:18":  return Sport.Specials;
                case "sr:sport:19":  return Sport.Snooker;
                case "sr:sport:20":  return Sport.TableTennis;
                case "sr:sport:21":  return Sport.Cricket;
                case "sr:sport:22":  return Sport.Darts;
                case "sr:sport:23":  return Sport.Volleyball;
                case "sr:sport:24":  return Sport.FieldHockey;
                case "sr:sport:25":  return Sport.Pool;
                case "sr:sport:26":  return Sport.Waterpolo;
                case "sr:sport:27":  return Sport.GaelicSports;
                case "sr:sport:28":  return Sport.Curling;
                case "sr:sport:29":  return Sport.Futsal;
                case "sr:sport:30":  return Sport.Olympics;
                case "sr:sport:31":  return Sport.Badminton;
                case "sr:sport:32":  return Sport.Bowls;
                case "sr:sport:33":  return Sport.Chess;
                case "sr:sport:34":  return Sport.BeachVolleyball;
                case "sr:sport:35":  return Sport.Netball;
                case "sr:sport:36":  return Sport.Athletics;
                case "sr:sport:37":  return Sport.Squash;
                case "sr:sport:38":  return Sport.RinkHockey;
                case "sr:sport:39":  return Sport.Lacrosse;
                case "sr:sport:40":  return Sport.Formula1;
                case "sr:sport:41":  return Sport.Bikes;
                case "sr:sport:42":  return Sport.DTM;
                case "sr:sport:43":  return Sport.AlpineSkiing;
                case "sr:sport:44":  return Sport.Biathlon;
                case "sr:sport:45":  return Sport.Bobsleigh;
                case "sr:sport:46":  return Sport.CrossCountry;
                case "sr:sport:47":  return Sport.NordicCombined;
                case "sr:sport:48":  return Sport.SkiJumping;
                case "sr:sport:49":  return Sport.Snowboard;
                case "sr:sport:50":  return Sport.SpeedSkating;
                case "sr:sport:51":  return Sport.Luge;
                case "sr:sport:52":  return Sport.Swimming;
                case "sr:sport:53":  return Sport.FinnishBaseball;
                case "sr:sport:54":  return Sport.Softball;
                case "sr:sport:55":  return Sport.HorseRacing;
                case "sr:sport:56":  return Sport.Schwingen;
                case "sr:sport:57":  return Sport.InlineHockey;
                case "sr:sport:58":  return Sport.Greyhound;
                case "sr:sport:59":  return Sport.RugbyLeague;
                case "sr:sport:60":  return Sport.BeachSoccer;
                case "sr:sport:61":  return Sport.Pesapallo;
                case "sr:sport:62":  return Sport.StreetHockey;
                case "sr:sport:63":  return Sport.WorldChampionship;
                case "sr:sport:64":  return Sport.Rowing;
                case "sr:sport:65":  return Sport.Freestyle;
                case "sr:sport:66":  return Sport.SnowboardcrossParallel;
                case "sr:sport:67":  return Sport.MotoGP;
                case "sr:sport:68":  return Sport.Moto2;
                case "sr:sport:69":  return Sport.Moto3;
                case "sr:sport:70":  return Sport.Nascar;
                case "sr:sport:71":  return Sport.PadelTennis;
                case "sr:sport:72":  return Sport.Canoeing;
                case "sr:sport:73":  return Sport.Horseball;
                case "sr:sport:74":  return Sport.Aquatics;
                case "sr:sport:75":  return Sport.Archery;
                case "sr:sport:76":  return Sport.Equestrian;
                case "sr:sport:77":  return Sport.Fencing;
                case "sr:sport:78":  return Sport.Gymnastics;
                case "sr:sport:79":  return Sport.Judo;
                case "sr:sport:80":  return Sport.ModernPentathlon;
                case "sr:sport:81":  return Sport.Sailing;
                case "sr:sport:82":  return Sport.Shooting;
                case "sr:sport:83":  return Sport.Taekwondo;
                case "sr:sport:84":  return Sport.Triathlon;
                case "sr:sport:85":  return Sport.Weightlifting;
                case "sr:sport:86":  return Sport.Wrestling;
                case "sr:sport:87":  return Sport.OlympicsYouth;
                case "sr:sport:88":  return Sport.MountainBike;
                case "sr:sport:89":  return Sport.Riding;
                case "sr:sport:90":  return Sport.Surfing;
                case "sr:sport:91":  return Sport.BMXRacing;
                case "sr:sport:92":  return Sport.CanoeSlalom;
                case "sr:sport:93":  return Sport.RhythmicGymnastics;
                case "sr:sport:94":  return Sport.Trampolining;
                case "sr:sport:95":  return Sport.SynchronizedSwimming;
                case "sr:sport:96":  return Sport.Diving;
                case "sr:sport:97":  return Sport.TrackCycling;
                case "sr:sport:98":  return Sport.BeachTennis;
                case "sr:sport:99":  return Sport.Sumo;
                case "sr:sport:100": return Sport.Superbike;
                case "sr:sport:101": return Sport.Rally;
                case "sr:sport:102": return Sport.FigureSkating;
                case "sr:sport:103": return Sport.FreestyleSkiing;
                case "sr:sport:104": return Sport.Skeleton;
                case "sr:sport:105": return Sport.ShortTrack;
                case "sr:sport:106": return Sport.ESportMythicalFootball;
                case "sr:sport:107": return Sport.ESport;
                case "sr:sport:108": return Sport.WorldLottery;
                case "sr:sport:109": return Sport.ESportCounterStrike;
                case "sr:sport:110": return Sport.ESportLeagueOfLegends;
                case "sr:sport:111": return Sport.ESportDota;
                case "sr:sport:112": return Sport.ESportStarCraft;
                case "sr:sport:113": return Sport.ESportHearthstone;
                case "sr:sport:114": return Sport.ESportHeroesOfTheStorm;
                case "sr:sport:115": return Sport.ESportWorldOfTanks;
                case "sr:sport:116": return Sport.Polo;
                case "sr:sport:117": return Sport.MMA;
                case "sr:sport:118": return Sport.ESportCallOfDuty;
                case "sr:sport:119": return Sport.ESportSmite;
                case "sr:sport:120": return Sport.ESportVainglory;
                case "sr:sport:121": return Sport.ESportOverwatch;
                case "sr:sport:122": return Sport.ESportWarCraft3;
                case "sr:sport:123": return Sport.ESportCrossfire;
                case "sr:sport:124": return Sport.ESportHalo;
                case "sr:sport:125": return Sport.ESportRainbowSix;
                case "sr:sport:126": return Sport.SepakTakraw;
                default:
                    return null;
            }
        }
    }
}