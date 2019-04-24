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
    public class IT3_PowerTube_CookController
    {
        //Uut
        private PowerTube _powerTube;
        private CookController _cookController;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private IDoor _door;
        private IDisplay _display;
        private IUserInterface _userInterface;
        private ILight _light;
        private ITimer _timer;


        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();

            _light = Substitute.For<ILight>();

            _display = Substitute.For<IDisplay>();

            _timer = Substitute.For<ITimer>();

            _userInterface = Substitute.For<IUserInterface>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
            
        }

        [Test]
        public void StartCooking_ValidParameters_PowerTubeStarted()
        {
            _cookController.StartCooking(50, 60);

            _output.Received(1).OutputLine("PowerTube works with 50 %");
        }

        [Test]
        public void TimerExpired_Cooking_PowerTubeOff()
        {
            _cookController.StartCooking(50, 60);

            _cookController.OnTimerExpired(null, EventArgs.Empty);

            _output.Received(1).OutputLine("PowerTube turned off");
        }

        [Test]
        public void Stop_Cooking_PowerTubeOff()
        {
            _cookController.StartCooking(50, 60);

            _cookController.Stop();

            _output.Received(1).OutputLine("PowerTube turned off");
        }

        [TestCase(701, 400)]
        [TestCase(49, 400)]
        [TestCase(-1, 400)]
        public void StartCooking_PowerNotValidtParameter_ExceptionThrown(int power, int time)
        {
            Assert.That(() => _cookController.StartCooking(power, time), Throws.TypeOf<ArgumentOutOfRangeException>().With.Message);
        }
    }
}
