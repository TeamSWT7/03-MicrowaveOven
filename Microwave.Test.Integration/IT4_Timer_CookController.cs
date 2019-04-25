using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT4_Timer_CookController
    {
        //Uut

        private Timer _timer;
        private CookController _cookController;

        //Fakes for UserInterface Paramaters
        private IDisplay _display;
        private IUserInterface _userInterface;
        private IPowerTube _powerTube;


        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface

            _display = Substitute.For<IDisplay>();

            _powerTube = Substitute.For<IPowerTube>();

            _userInterface = Substitute.For<IUserInterface>();

            #endregion
            
            //Used for test
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
            
        }

        

    }
}
