using System;
using System.Collections.Generic;

namespace brainfuck
{
    class Program
    {
        private const UInt32 _max_mem = 64 * 1024;

        static void Main(string[] args)
        {
            var programBuffer = new List<byte>();

            int c = -1;
            while ((c = Console.Read()) != -1)
            {
                if (c == '>' || c == '<' || c == '+' || c == '-' || c == '.' || c == ',' || c == '[' || c == ']')
                {
                    programBuffer.Add((byte)c);
                }
            }

            if (programBuffer.Count == 0)
            {
                return;
            }

            var program = programBuffer.ToArray();

            Console.WriteLine("\nStarting program in 3.. 2.. 1.. Now!\n");

            var memory = new byte[_max_mem];
            var stack = new Stack<int>();
            int pc = 0;
            UInt32 ptr = 0;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            while (pc < program.Length)
            {
                switch ((char)program[pc])
                {
                    case '>':
                        ptr = ptr != _max_mem ? ++ptr : 0;
                        ++pc;
                        break;
                    case '<':
                        ptr = ptr != 0 ? --ptr : _max_mem;
                        ++pc;
                        break;
                    case '+':
                        memory[ptr]++;
                        ++pc;
                        break;
                    case '-':
                        memory[ptr]--;
                        ++pc;
                        break;
                    case '.':
                        Console.Write((char)memory[ptr]);
                        ++pc;
                        break;
                    case ',':
                        c = Console.Read();
                        if (c != -1)
                        {
                            memory[ptr] = (byte)c;
                            ++pc;
                        }
                        break;
                    case '[':
                        if (memory[ptr] == 0)
                        {
                            int loopCount = 1;
                            int start_pc = pc;
                            while (loopCount != 0)
                            {
                                c = program[++pc];
                                if (c == '[')
                                {
                                    ++loopCount;
                                }
                                else if (c == ']')
                                {
                                    --loopCount;
                                }
                            }

                            pc++;
                        }
                        else
                        {
                            stack.Push(pc++);
                        }
                        break;
                    case ']':
                        pc = stack.Pop();
                        break;
                    default:
                        break;

                }
            }

            watch.Stop();

            Console.WriteLine($"\n\nProgram finished in {watch.ElapsedMilliseconds}...");

            Console.ReadLine();
        }
    }
}
