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
    public class IT6_StartCancelButton_UserInterface
    {
        // UUt
        private Button _startCancelButton;
        private UserInterface _userInterface;

        // Output
        private IOutput _output;

        // Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IDoor _door;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();
            _cookController = Substitute.For<ICookController>();

            #endregion

            // Used for test
            _output = Substitute.For<IOutput>();
            _door = new Door();
            _display = new Display(_output);
            _light = new Light(_output);
            _startCancelButton = new Button();
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _startCancelButton, _door, _display, _light,
                _cookController);
        }

        [Test]
        public void OnStartCancelPressed_myStateIsReady_NoOutput()
        {
            _startCancelButton.Press();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void OnStartCancelPressed_myStateIsSetPower_LightIsTurnedOff()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _light.TurnOn();
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnStartCancelPressed_myStateIsSetPower_DisplayIsCleared()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received(1).OutputLine("Display cleared");
        }

        [Test]
        public void OnStartCancelPressed_myStateIsSetTime_LightIsTurnedOn()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _userInterface.OnTimePressed(null, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void OnStartCancelPressed_myStateIsSetTime_DisplayIsCleared()
        {
            _userInterface.OnPowerPressed(new object(), EventArgs.Empty);
            _userInterface.OnTimePressed(new object(), EventArgs.Empty);
            _output.ClearReceivedCalls();

            _startCancelButton.Press();
            //_output.Received(1).OutputLine("Display cleared");
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }
    }
}