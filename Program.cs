using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Student
{
    public string Name;
    public string Group;
    public int Score;

    public Student(string name, string group, int score)
    {
        Name = name;
        Group = group;
        Score = score;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Завдання 1");
        Task1();

        Console.WriteLine("\nЗавдання 2");
        Task2();

        Console.WriteLine("\nЗавдання 3");
        Task3();

        Console.WriteLine("\nЗавдання 4");
        Task4();

        Console.WriteLine("\nЗавдання 5");
        Task5();
    }

    static List<string> GetWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder word = new StringBuilder();

        foreach (char ch in text)
        {
            if (char.IsLetter(ch))
            {
                word.Append(ch);
            }
            else if (word.Length > 0)
            {
                words.Add(word.ToString());
                word.Clear();
            }
        }

        if (word.Length > 0)
            words.Add(word.ToString());

        return words;
    }

    static Dictionary<int, int> CountWordLengthsDictionary(List<string> words)
    {
        Dictionary<int, int> counts = new Dictionary<int, int>();

        foreach (string word in words)
        {
            int length = word.Length;

            if (counts.TryGetValue(length, out int count))
                counts[length] = count + 1;
            else
                counts.Add(length, 1);
        }

        return counts;
    }

    static List<int> CountWordLengthsList(List<string> words, out List<int> counts)
    {
        List<int> lengths = new List<int>();
        counts = new List<int>();

        foreach (string word in words)
        {
            int length = word.Length;
            int index = lengths.IndexOf(length);

            if (index >= 0)
                counts[index]++;
            else
            {
                lengths.Add(length);
                counts.Add(1);
            }
        }

        return lengths;
    }

    static void Task1()
    {
        string text =
            "Алгоритм описує послідовність дій. Алгоритм можна записати словами, " +
            "а можна записати кодом. Кодом алгоритм стає тоді, коли його " +
            "виконує машина, а не людина. Одна задача — багато алгоритмів.";

        List<string> words = GetWords(text);
        Dictionary<int, int> dictionaryCounts = CountWordLengthsDictionary(words);
        List<int> listCounts;
        List<int> listLengths = CountWordLengthsList(words, out listCounts);

        foreach (KeyValuePair<int, int> item in dictionaryCounts)
        {
            if (item.Value >= 3)
                Console.WriteLine($"{item.Key}: {item.Value}");
        }

        Console.WriteLine($"Різних довжин: {dictionaryCounts.Count}");

        if (listLengths.Count != dictionaryCounts.Count)
            throw new InvalidOperationException("Результати підрахунку не збігаються.");

        for (int i = 0; i < listLengths.Count; i++)
        {
            if (!dictionaryCounts.TryGetValue(listLengths[i], out int count) ||
                count != listCounts[i])
                throw new InvalidOperationException("Результати підрахунку не збігаються.");
        }
    }

    static string ScoreBand(int score)
    {
        if (score <= 59)
            return "0–59";
        if (score <= 74)
            return "60–74";
        if (score <= 89)
            return "75–89";
        return "90–100";
    }

    static Dictionary<string, List<Student>> GroupStudentsDictionary(List<Student> students)
    {
        Dictionary<string, List<Student>> groups =
            new Dictionary<string, List<Student>>();

        foreach (Student student in students)
        {
            string key = ScoreBand(student.Score);

            if (!groups.TryGetValue(key, out List<Student> group))
            {
                group = new List<Student>();
                groups.Add(key, group);
            }

            group.Add(student);
        }

        return groups;
    }

    static List<string> GroupStudentsList(
        List<Student> students,
        out List<List<Student>> groups)
    {
        List<string> keys = new List<string>();
        groups = new List<List<Student>>();

        foreach (Student student in students)
        {
            string key = ScoreBand(student.Score);
            int index = keys.IndexOf(key);

            if (index < 0)
            {
                keys.Add(key);
                groups.Add(new List<Student>());
                index = keys.Count - 1;
            }

            groups[index].Add(student);
        }

        return keys;
    }

    static void Task2()
    {
        List<Student> students = new List<Student>
        {
            new Student("Коваленко", "КН-3/1", 88),
            new Student("Мельник", "КН-3/2", 64),
            new Student("Іщенко", "КН-3/1", 93),
            new Student("Бондар", "КН-3/1", 71),
            new Student("Савченко", "КН-3/2", 55),
            new Student("Ткачук", "КН-3/2", 90),
            new Student("Кравець", "КН-3/1", 76),
            new Student("Іванів", "КН-3/1", 48),
            new Student("Мороз", "КН-3/2", 82),
            new Student("Шевчук", "КН-3/1", 67),
            new Student("Бойко", "КН-3/2", 95),
            new Student("Кушнір", "КН-3/2", 73)
        };

        Dictionary<string, List<Student>> dictionaryGroups =
            GroupStudentsDictionary(students);
        List<List<Student>> listGroups;
        List<string> listKeys = GroupStudentsList(students, out listGroups);

        foreach (KeyValuePair<string, List<Student>> item in dictionaryGroups)
        {
            int total = 0;

            foreach (Student student in item.Value)
                total += student.Score;

            double average = (double)total / item.Value.Count;
            Console.WriteLine($"{item.Key}: {item.Value.Count}, середній бал {average:F2}");
        }

        if (listKeys.Count != dictionaryGroups.Count)
            throw new InvalidOperationException("Результати групування не збігаються.");

        for (int i = 0; i < listKeys.Count; i++)
        {
            if (!dictionaryGroups.TryGetValue(listKeys[i], out List<Student> group) ||
                group.Count != listGroups[i].Count)
                throw new InvalidOperationException("Результати групування не збігаються.");
        }
    }

    static List<int> RemoveDuplicatesPreservingOrder(List<int> numbers)
    {
        HashSet<int> seen = new HashSet<int>();
        List<int> result = new List<int>();

        foreach (int number in numbers)
        {
            if (seen.Add(number))
                result.Add(number);
        }

        return result;
    }

    static void Task3()
    {
        Random random = new Random(14);
        List<int> numbers = new List<int>();

        for (int i = 0; i < 25; i++)
            numbers.Add(random.Next(1, 8));

        HashSet<int> unordered = new HashSet<int>(numbers);
        List<int> ordered = RemoveDuplicatesPreservingOrder(numbers);

        Console.WriteLine($"Початковий список: {string.Join(", ", numbers)}");
        Console.WriteLine($"Без збереження порядку: {string.Join(", ", unordered)}");
        Console.WriteLine($"Зі збереженням порядку: {string.Join(", ", ordered)}");
    }

    static List<int> GetUniqueList(List<int> numbers)
    {
        List<int> unique = new List<int>();

        foreach (int number in numbers)
        {
            if (!unique.Contains(number))
                unique.Add(number);
        }

        return unique;
    }

    static List<int> UnionByNestedLoops(List<int> first, List<int> second)
    {
        List<int> union = GetUniqueList(first);

        foreach (int number in second)
        {
            bool found = false;

            foreach (int existing in union)
            {
                if (existing == number)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                union.Add(number);
        }

        return union;
    }

    static bool SameElements(HashSet<int> set, List<int> list)
    {
        if (set.Count != list.Count)
            return false;

        foreach (int number in set)
        {
            if (!list.Contains(number))
                return false;
        }

        return true;
    }

    static void Task4()
    {
        Random firstRandom = new Random(14);
        Random secondRandom = new Random(114);
        List<int> firstList = new List<int>();
        List<int> secondList = new List<int>();

        for (int i = 0; i < 30; i++)
        {
            firstList.Add(firstRandom.Next(1, 22));
            secondList.Add(secondRandom.Next(1, 22));
        }

        HashSet<int> firstSet = new HashSet<int>(firstList);
        HashSet<int> secondSet = new HashSet<int>(secondList);
        HashSet<int> hashUnion = new HashSet<int>(firstSet);
        hashUnion.UnionWith(secondSet);

        List<int> listUnion = UnionByNestedLoops(firstList, secondList);

        if (!SameElements(hashUnion, listUnion))
            throw new InvalidOperationException("Результати об'єднання не збігаються.");

        Console.WriteLine($"Розмір першого набору: {firstSet.Count}");
        Console.WriteLine($"Розмір другого набору: {secondSet.Count}");
        Console.WriteLine($"Об'єднання: {string.Join(", ", hashUnion)}");
        Console.WriteLine($"Розмір об'єднання: {hashUnion.Count}");
        Console.WriteLine($"Сума розмірів наборів: {firstSet.Count + secondSet.Count}");
        Console.WriteLine($"Різниця: {firstSet.Count + secondSet.Count - hashUnion.Count}");
    }

    static long MeasureTicks(Action operation)
    {
        operation();

        Stopwatch stopwatch = Stopwatch.StartNew();
        operation();
        stopwatch.Stop();

        return stopwatch.ElapsedTicks;
    }

    static double TicksToMilliseconds(long ticks)
    {
        return ticks * 1000.0 / Stopwatch.Frequency;
    }

    static void PrintBenchmark(string name, long listTicks, long setTicks)
    {
        double listMs = TicksToMilliseconds(listTicks);
        double setMs = TicksToMilliseconds(setTicks);
        double ratio = setTicks == 0 ? 0 : (double)listTicks / setTicks;

        Console.WriteLine(
            $"{name}: список {listMs:F2} мс, словник/множина {setMs:F2} мс, виграш {ratio:F2}x");
    }

    static void Task5()
    {
        Random random = new Random(2026);
        List<int> searchData = new List<int>();
        HashSet<int> searchSet = new HashSet<int>();
        List<int> queries = new List<int>();

        for (int i = 0; i < 100000; i++)
        {
            int value = random.Next(0, 20000);
            searchData.Add(value);
            searchSet.Add(value);
        }

        for (int i = 0; i < 10000; i++)
            queries.Add(random.Next(0, 20000));

        long listSearchTicks = MeasureTicks(() =>
        {
            int found = 0;
            foreach (int value in queries)
            {
                if (searchData.Contains(value))
                    found++;
            }
        });

        long setSearchTicks = MeasureTicks(() =>
        {
            int found = 0;
            foreach (int value in queries)
            {
                if (searchSet.Contains(value))
                    found++;
            }
        });

        PrintBenchmark("Перевірка наявності", listSearchTicks, setSearchTicks);

        List<string> words = new List<string>();

        for (int i = 0; i < 50000; i++)
            words.Add("слово" + random.Next(0, 500));

        long listFrequencyTicks = MeasureTicks(() =>
        {
            List<string> keys = new List<string>();
            List<int> counts = new List<int>();

            foreach (string word in words)
            {
                int index = keys.IndexOf(word);

                if (index >= 0)
                    counts[index]++;
                else
                {
                    keys.Add(word);
                    counts.Add(1);
                }
            }
        });

        long dictionaryFrequencyTicks = MeasureTicks(() =>
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (counts.TryGetValue(word, out int count))
                    counts[word] = count + 1;
                else
                    counts.Add(word, 1);
            }
        });

        PrintBenchmark("Підрахунок частот", listFrequencyTicks, dictionaryFrequencyTicks);

        List<int> duplicateData = new List<int>();

        for (int i = 0; i < 20000; i++)
            duplicateData.Add(random.Next(0, 1000));

        long listDuplicatesTicks = MeasureTicks(() =>
        {
            List<int> unique = new List<int>();

            foreach (int value in duplicateData)
            {
                if (!unique.Contains(value))
                    unique.Add(value);
            }
        });

        long setDuplicatesTicks = MeasureTicks(() =>
        {
            HashSet<int> unique = new HashSet<int>(duplicateData);
        });

        PrintBenchmark("Вилучення дублікатів", listDuplicatesTicks, setDuplicatesTicks);

        List<int> first = new List<int>();
        List<int> second = new List<int>();

        for (int i = 0; i < 5000; i++)
        {
            first.Add(random.Next(0, 1500));
            second.Add(random.Next(0, 1500));
        }

        long listIntersectionTicks = MeasureTicks(() =>
        {
            List<int> intersection = new List<int>();

            foreach (int value in first)
            {
                if (second.Contains(value) && !intersection.Contains(value))
                    intersection.Add(value);
            }
        });

        long setIntersectionTicks = MeasureTicks(() =>
        {
            HashSet<int> intersection = new HashSet<int>(first);
            intersection.IntersectWith(second);
        });

        PrintBenchmark("Спільні елементи", listIntersectionTicks, setIntersectionTicks);
    }
}
