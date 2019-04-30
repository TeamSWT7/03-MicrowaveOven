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
    public class IT2_Display_UserInterface
    {
        //Uut
        private Display _display;
        private UserInterface _userInterface;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private IDoor _door;
        private ILight _light;
        private ICookController _cookController;


        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();

            _light = Substitute.For<ILight>();

            _cookController = Substitute.For<ICookController>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light, _cookController);
        }

        #region Display/UI integrationtest


        #region Power button pressed tests

        [Test]
        public void OnPowerPressed_MicrowaveIsOff_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            //50 W is default
            _output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnPowerPressedTwice_MicrowaveIsOff_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 100 W");
        }

        [Test]
        public void OnPowerPressedUntilOverflow_MicrowaveIsOff_OutputIsCorrect()
        {
            for (int i = 50; i <= 700; i += 50)
            {
                _userInterface.OnPowerPressed(null, EventArgs.Empty);
            }

            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _output.Received(2).OutputLine("Display shows: 50 W");
        }

        #endregion

        #region Display clearing tests

        [Test]
        public void StartCancelButtonPressed_MicrowaveIsReady_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void StartCancelButtonPressed_Cooking_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void DoorOpened_MicrowaveIsReady_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnDoorOpened(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void DoorOpened_TimeHasBeenPressed_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnDoorOpened(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void CookingIsDone_Cooking_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _userInterface.CookingIsDone();

            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void DoorOpened_Cooking_DisplayCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _userInterface.OnDoorOpened(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display cleared");
        }

        #endregion

        #region Timer button pressed tests

        [Test]
        public void OnTimerPressed_PowerPressed_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void OnTimerPressedTwice_PowerPressed_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 02:00");
        }

        #endregion

        #endregion
    }
}
