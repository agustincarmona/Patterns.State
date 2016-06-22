using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatronState
{
    class Program
    {
        static void Main(string[] args)
        {
            Car car = new Car(20);

            car.State.Accelerate();
            car.State.Start();
            car.State.Accelerate();
            car.State.Accelerate();
            car.State.Accelerate();
            car.State.Accelerate();
            car.State.Stop();
            car.State.Stop();
            car.State.Stop();

            Console.Read();
        }
    }

    public interface CarState
    {
        void Accelerate();
        void Stop();
        void Start();
    }

    public class OffCarState : CarState
    {
        private Car car;

        public OffCarState(Car car)
        {
            this.car = car;
        }

        public void Accelerate()
        {
            Console.WriteLine($"[OffCarState.Accelerate] Velocidad actual: {car.CurrentSpeed}. Combustible restante: {car.CurrentGasoline}");
            Console.WriteLine("[OffCarState.Accelerate] ERROR: El vehiculo esta apagado. Efectue el contacto para iniciar");
        }

        public void Stop()
        {
            Console.WriteLine("[OffCarState.Stop] ERROR: El vehiculo esta apagado. Efectue el contacto para iniciar");
        }

        public void Start()
        {
            if (car.CurrentGasoline > 0)
            {
                Console.WriteLine("[OffCarState.Start] El vehiculo se encuentra ahora PARADO");
                car.State = new StopCarState(car);
                car.CurrentSpeed = 0;
            }
            else
            {
                Console.WriteLine("[OffCarState.Start] El vehiculo se encuentra sin combustible");
                car.State = new WithoutGasolineCarState(car);
            }
        }
    }

    public class StopCarState : CarState
    {
        private Car car;

        public StopCarState(Car car)
        {
            this.car = car;
        }

        public void Accelerate()
        {
            Console.WriteLine($"[StopCarState.Accelerate] Velocidad actual: {car.CurrentSpeed}. Combustible restante: {car.CurrentGasoline}");

            if (car.CurrentGasoline > 0)
            {
                Console.WriteLine("[StopCarState.Accelerate] El vehiculo se encuentra ahora EN MARCHA");
                car.State = new OnCarState(car);
                car.IncrementSpeed(10);
                car.IncrementGasoline(-10);
            }
            else
            {
                Console.WriteLine("[StopCarState.Accelerate] El vehiculo se encuentra ahora SIN COMBUSTIBLE");
                car.State = new WithoutGasolineCarState(car);
            }
        }

        public void Stop()
        {
            Console.WriteLine("[StopCarState.Stop] ERROR: El vehiculo ya se encuentra detenido");
        }

        public void Start()
        {
            Console.WriteLine("[StopCarState.Start] El vehiculo se encuentra ahora APAGADO");
            car.State = new OffCarState(car);
        }
    }

    public class OnCarState : CarState
    {
        private const int SPEED_MAX = 200;

        private Car car;

        public OnCarState(Car car)
        {
            this.car = car;
        }

        public void Accelerate()
        {
            Console.WriteLine($"[OnCarState.Accelerate] Velocidad actual: {car.CurrentSpeed}. Combustible restante: {car.CurrentGasoline}");

            if (car.CurrentGasoline > 0)
            {
                if (car.CurrentSpeed >= SPEED_MAX)
                {
                    Console.WriteLine("[OnCarState.Accelerate] ERROR: El coche ha alcanzado su velocidad maxima");
                    car.IncrementGasoline(-10);
                }
                else
                {
                    Console.WriteLine("[OnCarState.Accelerate] Aumenta la velocidad y decrementa la gasolina");
                    car.IncrementSpeed(10);
                    car.IncrementGasoline(-10);
                }
            }
            else
            {
                Console.WriteLine("[OnCarState.Accelerate] El vehiculo se ha quedado sin combustible");
                car.State = new WithoutGasolineCarState(car);
            }
        }

        public void Stop()
        {
            Console.WriteLine("[OnCarState.Stop] El vehiculo reduce la velocidad");
            car.IncrementSpeed(-10);

            if (car.CurrentSpeed <= 0)
            {
                Console.WriteLine("[OnCarState.Stop] El vehiculo se encuentra ahora PARADO");
                car.State = new StopCarState(car);
            }
        }

        public void Start()
        {
            Console.WriteLine("[OnCarState.Start] ERROR: No se puede cortar el contacto en marcha!");
        }
    }

    public class WithoutGasolineCarState : CarState
    {
        private Car car;

        public WithoutGasolineCarState(Car car)
        {
            this.car = car;
        }

        public void Accelerate()
        {
            Console.WriteLine("[WithoutGasolineCarState.Accelerate] ERROR: El vehiculo se encuentra sin combustible");
        }

        public void Stop()
        {
            Console.WriteLine("[WithoutGasolineCarState.Stop] El vehiculo se encuentra sin combustible");
            this.car.State = new StopCarState(this.car);
        }

        public void Start()
        {
            Console.WriteLine("[WithoutGasolineCarState.Start] ERROR: El vehiculo se encuentra sin combustible");
        }
    }

    public class Car
    {
        private int currentGasoline = 0;

        public CarState State { get; set; }

        public int CurrentSpeed { get; set; }

        public int CurrentGasoline
        {
            get { return currentGasoline; }
        }

        public Car(int gasoline)
        {
            this.currentGasoline = gasoline;
            this.State = new OffCarState(this);
        }

        public void IncrementSpeed(int kmh)
        {
            this.CurrentSpeed += kmh;
        }

        public void IncrementGasoline(int deciliter)
        {
            this.currentGasoline += deciliter;
        }
    }

}
