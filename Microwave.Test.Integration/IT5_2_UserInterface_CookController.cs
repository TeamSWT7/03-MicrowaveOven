using System;
using System.Runtime.InteropServices;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT5_2_UserInterface_CookController
    {
        // UUT
        private UserInterface _userInterface;
        private CookController _cookController;

        // Output
        private IOutput _output;

        // Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private IDoor _door;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
        private IPowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface and CookController

            // UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            
            #endregion

            _output = Substitute.For<IOutput>();

            _light=new Light(_output);

            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light,
                _cookController);
        }

        [TestCase(0, 50)]
        [TestCase(1, 100)]
        [TestCase(2, 150)]
        [TestCase(3, 200)]
        [TestCase(4, 250)]
        [TestCase(5, 300)]
        [TestCase(6, 350)]
        public void Press_SetTimeState_CookingStartsCookingWithCorrectPower(int timesPowerButtonPressed, int power)
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            for (int i = 0; i < timesPowerButtonPressed; i++)
            {
                _userInterface.OnPowerPressed(null, EventArgs.Empty);
            }
            _userInterface.OnTimePressed(null, EventArgs.Empty);

            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);
            _output.Received(1).OutputLine($"PowerTube works with {power} W");
        }
    }
}