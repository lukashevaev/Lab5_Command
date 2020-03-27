using System;
using System.Collections.Generic;

namespace Lab5_command
{
    /// <summary>
    /// Конструктор класса Command (interface)
    /// <param name=" Execute">Метод для выполнения действия</param>
    /// <param name="Undo">Метод для отмены действия</param>
    /// </summary>
    /// <remarks>Интерфейс команды</remarks>
    interface Command
    {
        void Execute();
        void Undo();
    }
    /// <summary>
    /// Конструктор класса Device (Reciever)
    /// <param name="TurnOn">Метод ,отвечающий за включение устройства</param>
    /// <param name="TurnOff">Метод ,отвечающий за выключение устройства</param>
    /// </summary>
    /// <remarks>Reciever команды-некий девайс, на который поступают команды</remarks>
    class Device
    {
        public void TurnOn()
        {
            Console.WriteLine("Включение");
        }

        public void TurnLOW()
        {
            Console.WriteLine("Выключение");
        }
    }
    /// <summary>
    /// Конструктор класса CommandOn (concrete command)
    /// <param name="CommandOn"></param>
    /// <param name=" Execute">Метод для выполнения действия</param>
    /// <param name="Undo">Метод для отмены действия</param>
    /// </summary>
    /// <remarks>Реализация команды</remarks>
    class CommandOn : Command
    {
        Device Device;
        public CommandOn(Device DeviceSet)
        {
            Device = DeviceSet;
        }
        public void Execute()
        {
            Device.TurnOn();
        }
        public void Undo()
        {
            Device.TurnLOW();
        }
    }
    /// <summary>
    /// Конструктор класса Power 
    /// <param name="Power">Определяет значение переменной level </param>
    /// <param name="RaisePow">Метод отвечает за повышение мощности</param>
    /// <param name="LessPow">Метод отвечает за уменьшение мощности</param>
    /// </summary>
    /// <remarks>Команда для изменения мощности</remarks>
    class Power
    {
        public const int LOW = 0;
        public const int TEMP = 4;
        public const int HIGH = 10;
        private int level;

        public Power()
        {
            level = TEMP;
        }

        public void RaisePow()
        {
            if (level < HIGH)
                level++;
            Console.WriteLine("Мощность {0}", level);
        }
        public void LessPow()
        {
            if (level > LOW)
                level--;
            Console.WriteLine("Мощность {0}", level);
        }
    }
    /// <summary>
    /// Конструктор класса PowerCommand (concrete command)
    /// <param name="Execute">Метод для выполнения действия</param>
    /// <param name="Undo">Метод для отмены действия команды</param>
    /// </summary>
    /// <remarks>Реализация команды</remarks>
    class PowerCommand : Command
    {
        Power Power;
        public PowerCommand(Power v)
        {
            Power = v;
        }
        public void Execute()
        {
            Power.RaisePow();
        }
        public void Undo()
        {
            Power.LessPow();
        }
    }
    /// <summary>
    /// Конструктор класса NoCommand (concrete command)
    /// <param name="Execute">Метод для выполнения действия</param>
    /// <param name="Undo">Метод для отмены действия команды</param>
    /// </summary>
    /// <remarks>Реализация команды</remarks>
    class NoCommand : Command
    {
        public void Execute() {}
        public void Undo() {}
    }
    /// <summary>
    /// Конструктор класса MultiPult (invoker)
    /// <param name="buttons">массив команд</param>
    /// <param name="commandsHistory">каждый следующий добавленный элемент помещается поверх предыдущего</param>
    /// <param name="MultiPult">Задает массив,создаем историю команд</param>
    /// <param name="SetCommand">Метод для действия,устанавливается команда</param>
    /// <param name="PressButton">Метод для действия с кнопкой</param>
    /// <param name="PressUndoButton">Метод для отмены действия</param> 
    /// </summary>
    /// <remarks>Инициатор команды-вызывает команду</remarks>
    class MultiPult
    {
        Command[] buttons;
        Stack<Command> commandsHistory;  

        public MultiPult()
        {
            buttons = new Command[2];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new NoCommand();
            }
            commandsHistory = new Stack<Command>();
        }

        public void SetCommand(int number, Command com)
        {
            buttons[number] = com;
        }

        public void PressButton(int number)
        {
            buttons[number].Execute();
            commandsHistory.Push(buttons[number]);//добавляет выполненную команду в стек на первое место
        }
        public void PressUndoButton()
        {
            if (commandsHistory.Count > 0)
            {
                Command undoCommand = commandsHistory.Pop(); //извлекает и возвращает первый элемент из стека
                undoCommand.Undo();
            }
        }
    }
    /// <summary>
    /// Точка входа для приложения
    /// <param name="Device">Переменная для включения</param>
    ///  <param name="Power">Переменная для увеличения мощности</param>
    ///  <param name="mPult">Переменная для работы с кнопками:включение или мощность</param>
    /// </summary>
    /// <remarks>Сторона клиента</remarks>
    class Program
    {
        static void Main(string[] args)
        {
            Device Device = new Device();
            Power Power = new Power();
            MultiPult mPult = new MultiPult();

            int i = 0;
             Console.WriteLine("Введите команду");
             string command = Console.ReadLine();
            if (command == "on")
            {
                mPult.SetCommand(0, new CommandOn(Device));
                Console.WriteLine("Устройство включено");
            }
            else if (command == "up")
            {
                while (i < 6)
                {
                    mPult.SetCommand(1, new PowerCommand(Power));
                    mPult.PressButton(1);
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Неправильная команда");
            }
            Console.Read();

            
        }
    }
}
