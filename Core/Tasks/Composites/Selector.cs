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

using CZToolKit.VM;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Selector")]
    [NodeTitle("选择执行")]
    [NodeTooltip("依次执行，直到某一行为成功，则返回成功，若所有行为都失败，则返回失败")]
    [NodeMenu("Composite/Selector")]
    public class Selector : Task
    {
    }

    [ViewModel(typeof(Selector))]
    public class SelectorVM : CompositeTaskVM
    {
        private int currentIndex;

        public SelectorVM(Selector model) : base(model)
        {
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
                Stopped(true);
            else
            {
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
            if (result)
                Stopped(true);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                Stopped(false);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}