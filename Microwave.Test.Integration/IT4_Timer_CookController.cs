using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

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

        [TestCase(60)]
        [TestCase(20)]
        public void StartCooking_ValidParameters_TimeStarted(int time)
        {
            _cookController.StartCooking(50, time);

            Assert.That(_timer.TimeRemaining, Is.EqualTo(time));

            _cookController.Stop();
        }

        [TestCase(-60)] // You can input any number to timer - not good
        [TestCase(0)]
        public void StartCooking_NonValidParameters_TimeStarted(int time)
        {
            _cookController.StartCooking(50, time);

            Assert.That(_timer.TimeRemaining, Is.EqualTo(time));

            _cookController.Stop();
        }

        [Test]
        public void OnTimerTick_TimerIsRunning_DisplayCorrectParam()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _cookController.StartCooking(50, 60000);

            _timer.TimerTick += (sender, args) => pause.Set();

            Assert.That(pause.WaitOne(1100));

            Assert.That(_timer.TimeRemaining, Is.EqualTo(((60*1000)-1000)));

            _display.Received(1).ShowTime(0, 59);

            _cookController.Stop();
        }

        [TestCase(1000)]
        [TestCase(2000)]
        public void Stop_TimerRunning_TimerStopped(int sleep)
        {
            _cookController.StartCooking(50, 10000);

            _cookController.Stop();

            Assert.That(_timer.TimeRemaining, Is.EqualTo(10000));

            Thread.Sleep(sleep);

            Assert.That(_timer.TimeRemaining, Is.EqualTo(10000));

            _cookController.Stop();
        }

        [Test]
        public void Expire_TimerRunsOut_CookingIsDoneIsCalled()
        {
            _cookController.StartCooking(50, 10000);


            Assert.That(_timer.TimeRemaining, Is.EqualTo(10000));

            Thread.Sleep(10000);

            Assert.That(_timer.TimeRemaining, Is.EqualTo(0));

            _userInterface.Received(1).CookingIsDone();

            _cookController.Stop();
        }

    }
}
