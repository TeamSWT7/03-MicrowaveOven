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

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_Light_UserInterface
    {
        //Uut
        private Light _light;
        private UserInterface _userInterface;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private IDoor _door;
        private IDisplay _display;
        private ICookController _cookController;


        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();

            _display = Substitute.For<IDisplay>();

            _cookController = Substitute.For<ICookController>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light, _cookController);
        }

        #region Light integrationtest

        [Test]
        public void CookingIsDone_LightIsOn_LightIsTurnedOff()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);
            
            _userInterface.CookingIsDone();

            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnStartCancelPressed_LightIsOff_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned on");
        }



        [Test]
        public void OnDoorClosed_LightIsOn_LightIsTurnedOff()
        {
            _userInterface.OnDoorOpened(null, EventArgs.Empty);

            _userInterface.OnDoorClosed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnDoorClosed_LightIsOff_LightIsTurnedOn()
        {
            _userInterface.OnDoorOpened(null, EventArgs.Empty);
            
            _output.Received(1).OutputLine("Light is turned on");
        }

        #endregion
    }
}
