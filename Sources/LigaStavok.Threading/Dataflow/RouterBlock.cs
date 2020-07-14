using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace LigaStavok.Threading.Dataflow
{

    public class RouterBlock<TInput, TOutput1, TOutput2, TOutput3> 
        where TOutput1 : class
        where TOutput2 : class
        where TOutput3 : class
    {
        private readonly ActionBlock<TInput> input;
        private readonly BufferBlock<TOutput1> output1;
        private readonly BufferBlock<TOutput2> output2;
        private readonly BufferBlock<TOutput3> output3;

        public ITargetBlock<TInput> Input => input; 
        public ISourceBlock<TOutput1> Output1 => output1; 
        public ISourceBlock<TOutput2> Output2 => output2;
        public ISourceBlock<TOutput3> Output3 => output3;


        public RouterBlock(
            Func<TInput, Tuple<TOutput1, TOutput2, TOutput3>> combinedTransform)
        {
            input = new ActionBlock<TInput>(
                value =>
                {
                    var result = combinedTransform(value);

                    if (result.Item1 != null)
                        output1.Post(result.Item1);
                    if (result.Item2 != null)
                        output2.Post(result.Item2);
                    if (result.Item3 != null)
                        output3.Post(result.Item3);
                });

            output1 = new BufferBlock<TOutput1>();
            output2 = new BufferBlock<TOutput2>();
            output3 = new BufferBlock<TOutput3>();

            // TODO handle fault in input correctly
            input.Completion.ContinueWith(
                _ =>
                {
                    output1.Complete();
                    output2.Complete();
                    output3.Complete();
                });
        }

        public RouterBlock(
            Func<TInput, TOutput1> transform1,
            Func<TInput, TOutput2> transform2,
            Func<TInput, TOutput3> transform3)
            : this(x => Tuple.Create(transform1(x), transform2(x), transform3(x)))
        { }
    }
}
