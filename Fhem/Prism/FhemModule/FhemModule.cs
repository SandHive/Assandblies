using System;
using Prism.Modularity;
using Prism.Regions;
using Sand.Fhem.Prism.FhemModule.Views;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Prism.FhemModule
{
    public class FhemModule : IModule
    {
        //---------------------------------------------------------------------
        #region Fields

        private IRegionViewRegistry  m_regionViewRegistry;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        public FhemModule( IRegionViewRegistry a_regionViewRegistry )
        {
            m_regionViewRegistry = a_regionViewRegistry;
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region IModule Members

        public void Initialize()
        {
            m_regionViewRegistry.RegisterViewWithRegion( "MainRegion", typeof( FhemExplorerView ) );
        }

        //-- IModule Members
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------