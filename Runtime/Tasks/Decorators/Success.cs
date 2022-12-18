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
using CZToolKit.Core.ViewModel;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeMenu("Decorator/Success")]
    public class Success : Task { }

    [ViewModel(typeof(Success))]
    public class SuccessVM : DecoratorTaskVM
    {
        public SuccessVM(Success model) : base(model) { }

        protected override void DoStart()
        {
            base.DoStart();
            Child.Start();
        }

        protected override void DoStop()
        {
            base.DoStop();
            Child.Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool succeeded)
        {
            Stopped(true);
        }
    }
}
