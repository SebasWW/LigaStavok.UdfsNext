using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LigaStavok.Threading.Dataflow
{
    public class SplitBlock<TInput, TOutputLeft, TOutputRight> 
    {
        private ActionBlock<TInput> input;
        private BufferBlock<TOutputLeft> leftOutput;
        private BufferBlock<TOutputRight> rightOutput;

        public ITargetBlock<TInput> Input { get { return input; } }
        public ISourceBlock<TOutputLeft> LeftOutput { get { return leftOutput; } }
        public ISourceBlock<TOutputRight> RightOutput { get { return rightOutput; } }


		public SplitBlock(
            Func<TInput, Tuple<TOutputLeft, TOutputRight>> combinedTransform)
        {
            input = new ActionBlock<TInput>(
                value =>
                {
                    var result = combinedTransform(value);
                    leftOutput.Post(result.Item1);
                    rightOutput.Post(result.Item2);
                });

            leftOutput = new BufferBlock<TOutputLeft>();
            rightOutput = new BufferBlock<TOutputRight>();

            // TODO handle fault in input correctly
            input.Completion.ContinueWith(
                _ =>
                {
                    leftOutput.Complete();
                    rightOutput.Complete();
                });
        }

        public SplitBlock(
            Func<TInput, TOutputLeft> leftTransform,
            Func<TInput, TOutputRight> rightTransform)
            : this(x => Tuple.Create(leftTransform(x), rightTransform(x)))
        { }
	}
}
