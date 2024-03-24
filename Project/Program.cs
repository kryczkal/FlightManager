﻿using System.Numerics;
using DataTransformation;
using NetworkSourceSimulator;

namespace projob;

public static class Program
{
    public static void Main(string[] args)
    {
        var networkSourceManager = new NetworkSourceManager(
            new NetworkSourceSimulator.NetworkSourceSimulator(
                Settings.LoadPath,
                Settings.minSimulationOffset,
                Settings.maxSimulationOffset),
            true
        );

        networkSourceManager.RunParallel();
        GuiManager.RunParallel();
        ConsoleWork(networkSourceManager);
    }

    public static void ConsoleWork(NetworkSourceManager networkSourceManager)
    {
        var serializer = new SerializerFactory().CreateProduct("json");
        if (serializer == null) throw new Exception("Serializer not found");

        var running = true;
        while (running)
        {
            Console.WriteLine("Enter a command: ");
            var command = Console.ReadLine();
            switch (command)
            {
                case "exit":
                    running = false;
                    break;
                case "print":
                    DataBaseManager.MakeSnapshot(serializer);
                    break;
            }
        }
    }
}