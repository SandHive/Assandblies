using System.Windows;
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelWidgetGridCell : Border
    {
        //---------------------------------------------------------------------
        #region Properties



        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        public SandPanelWidgetGridCell( Size size )
        {
            base.Width = size.Width;
            base.Height = size.Height;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------