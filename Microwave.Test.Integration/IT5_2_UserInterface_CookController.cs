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
            _light = Substitute.For<ILight>();

            // CookController
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();

            #endregion

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light,
                _cookController);
        }


    }
}