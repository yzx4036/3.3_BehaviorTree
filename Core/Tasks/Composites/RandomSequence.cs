#region 注 释

/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */

#endregion

using System;
using CZToolKit.VM;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/RandomSequence")]
    [NodeTitle("随机顺序")]
    [NodeTooltip("随机顺序执行，直到某一行为失败，则返回失败，若所有行为都成功，则返回成功")]
    [NodeMenu("Composite/Random Sequence")]
    public class RandomSequence : Task
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSequence))]
    public class RandomSequenceVM : CompositeTaskVM
    {
        private int currentIndex;
        private Random random;

        public RandomSequenceVM(RandomSequence model) : base(model)
        {
            this[nameof(RandomSequence.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);

            random = new Random(model.randomSeed);
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
                Stopped(true);
            else
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    var index = random.Next(0, i + 1);
                    var temp = Children[i];
                    Children[i] = Children[index];
                    Children[index] = temp;
                }

                currentIndex = 0;
                Children[currentIndex].Start();
            }
        }

        protected override void DoStop()
        {
            Children[currentIndex].Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (!result)
                Stopped(false);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                Stopped(true);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}