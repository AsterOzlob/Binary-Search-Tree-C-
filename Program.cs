/*
Разработать программу, которая содержит информацию о дисциплинах,
читаемых преподавателем студентам университета в течение учебного года.
Сведения о нагрузке преподавателя за учебный год содержат:
- название дисциплины;
- семестр проведения занятия;
- количество студентов;
- количество часов аудиторных лекций;
- количество часов аудиторных практических работ;
- вид контроля знаний студентов (зачет/экзамен).
Программа должна обеспечивать:
- начальное формирование данных о дисциплинах, читаемых
преподавателем, в виде двоичного дерева;
- добавление данных о дисциплинах;
- удаление данных о дисциплинах;
- вывод данных о дисциплинах по наименованию;
- вывод всех дисциплин, составляющих нагрузку преподавателя.
Программа должна обеспечивать диалог с помощью меню и контроль
ошибок при вводе.
*/

using System;

//Класс, хранящий данные о дисциплине
class Discipline
{
    //Атрибуты класса
    private string name; //Наименование дисциплины
    private int semester; //Семестр проведения занятий
    private int students; //Количество студентов
    private int lectures; //Количество часов аудиторных лекций
    private int practices; //Количество часов аудиторных занятий
    private int controlType; //Вид контроля знаний студeнтов

    public string Name { get => name; set => name = value; }
    public int Semester { get => semester; set => semester = value; }
    public int Students { get => students; set => students = value; }
    public int Lectures { get => lectures; set => lectures = value; }
    public int Practices { get => practices; set => practices = value; }
    public int ControlType { get => controlType; set => controlType = value; }

    //Конструктор
    public Discipline(string name, int semester, int students,
        int lectures, int practices, int controlType)
    {
        Name = name;
        Semester = semester;
        Students = students;
        Lectures = lectures;
        Practices = practices;
        ControlType = controlType;
    }

}

//Класс, описывающий узел дерева
class Node
{
    //Атрибуты класса
    private Discipline data;
    private Node? leftNode; //Левый потомок дерева
    private Node? rightNode; //Правый потомок дерева

    internal Discipline Data { get => data; set => data = value; }
    internal Node? LeftNode { get => leftNode; set => leftNode = value; }
    internal Node? RightNode { get => rightNode; set => rightNode = value; }

    //Конструктор
    public Node(Discipline data)
    {
        Data = data;
        LeftNode = null;
        RightNode = null;
    }
}

//Класс, описывающий преподавателя
class TeacherLoad
{
    //Атрибуты класса
    private string teacherName; //Имя преподавателя
    private Node? root; //Вершина дерева

    public string TeacherName { get => teacherName; set => teacherName = value; }
    internal Node? Root { get => root; set => root = value; }

    //Конструктор
    public TeacherLoad(string name)
    {
        teacherName = name;
        Root = null;
    }

    //Метод добавления дисциплины в дерево
    //Аргументы: экземпляр класса Discipline
    public void AddDiscipline(Discipline data)
    {
        if (Root == null) //Если дерево не существует
        {
            Root = new Node(data); //Создаём корень дерева
        }
        else
        {
            AddNode(Root, data); //Вызов метода добавления узла
        }
    }

    //Рекурсивный метод создания узла дерева
    //Аргументы: ссылка на корень дерева, экзмепляр класса Discipline
    private void AddNode(Node node, Discipline data)
    {
        //Сортировка узлов дерева по алфавиту
        //Если название < чем у текущего узла
        if (data.Name.CompareTo(node.Data.Name) < 0)
        {
            if (node.LeftNode == null)
            {
                node.LeftNode = new Node(data);
            }
            else
            {
                AddNode(node.LeftNode, data);
            }
        }
        else //Если название >= чем у текущего узла
        {
            if (node.RightNode == null)
            {
                node.RightNode = new Node(data);
            }
            else
            {
                AddNode(node.RightNode, data);
            }
        }
    }

    //Метод удаления дисциплины
    public void DeleteDiscipline(string name)
    {
        Root = DeleteNode(Root, name); //Вызов метода удаления узла
    }

    //Рекурсивный метод удаления узла дерева
    //Аргументы: ссылка на узел дерева, наименование дисциплины
    //Возвращает ссылку на корень дерева
    private Node DeleteNode(Node node, string name)
    {
        if (node == null)
        {
            return null;
        }

        //Поиск по алфавиту
        if (name.CompareTo(node.Data.Name) < 0)
        {
            node.LeftNode = DeleteNode(node.LeftNode, name);
        }
        else if (name.CompareTo(node.Data.Name) > 0)
        {
            node.RightNode = DeleteNode(node.RightNode, name);
        }
        else //Если нашлась нужна дисциплина
        {
            //Если потомков нет
            if (node.LeftNode == null && node.RightNode == null)
            {
                node = null; //Удаление узла
            }
            //Если есть только 1 потомок, он заменяет удаляемый узел
            else if (node.LeftNode == null)
            {
                node = node.RightNode;
            }
            else if (node.RightNode == null)
            {
                node = node.LeftNode;
            }
            //Если есть оба потомка, то ищем лист в правом поддереве,
            //Его данные копируем в удаляемый узел, а этот узел удаляем
            else
            {
                Node temp = node.RightNode;
                while (temp.LeftNode != null)
                {
                    temp = temp.LeftNode;
                }

                node.Data = temp.Data;
                node.RightNode = DeleteNode(node.RightNode, temp.Data.Name);
            }
        }

        return node;
    }

    //Метод поиска дисциплины по наименованию
    //Аргументы: наименование дисциплины
    public void SearchByName(string name)
    {
        Node node = FindNode(Root, name);

        if (node == null)
        {
            Console.WriteLine("\nДисциплина не найдена.\n");
        }
        else
        {
            PrintNode(node);
        }
    }

    //Рекурсивный метод поиска дисциплины по наименованию
    //Аргументы: ссылка на узел дерева, наименование дисциплины
    //Возвращает найденный узел
    private Node FindNode(Node node, string name)
    {
        if (node == null)
        {
            return null;
        }

        if (name.CompareTo(node.Data.Name) < 0) //Левое поддерево
        {
            return FindNode(node.LeftNode, name);
        }
        else if (name.CompareTo(node.Data.Name) > 0) //Правое поддерево
        {
            return FindNode(node.RightNode, name);
        }
        else //Если нашлось
        {
            return node;
        }
    }

    //Метод вывода бинарного дерева в консоль
    public void PrintAll()
    {
        if (root == null)
        {
            Console.WriteLine("\nСписок дисциплин преподавателя \"" + teacherName + "\" пуст\n");
            return;
        }
        Console.WriteLine("\nСписок дисциплин преподавателя " + teacherName + ":");
        PrintTree(Root);
        return;
    }

    //Метод вывода дерева в формате левое поддерево-правое поддерево
    //Аргументы: ссылка на корень дерева
    private void PrintTree(Node node)
    {
        if (node != null)
        {
            PrintTree(node.LeftNode);
            PrintNode(node);
            PrintTree(node.RightNode);
        }
    }

    //Метод вывода данных, хранимых в узле дерева
    //Аргумент: ссылка на выводимый узел
    private void PrintNode(Node node)
    {
        Console.Write("\nНаименование дисциплины: {0}\n" +
            "Количество студентов: {1}\n" +
            "Семестр: {2}\n" +
            "Лекционнные часы: {3}\n" +
            "Практические часы: {4}\n",
            node.Data.Name, node.Data.Students, node.Data.Semester,
            node.Data.Lectures, node.Data.Practices);

        if (node.Data.ControlType == 0)
            Console.WriteLine("Контроль знаний: Экзамен\n");
        else
            Console.WriteLine("Контроль знаний: Зачёт\n");
    }
}

class Program
{
    static void Main()
    {
        //Заголовок программы
        Console.WriteLine("Программа для хранения нагрузки преподавателей\n");

        string teacherName = "Иванов";

        //Словарь преподавателей
        Dictionary<string, TeacherLoad> teachers = new Dictionary<string, TeacherLoad>();

        //Формирование базовых данных (для тестирования программы)
        teachers.Add(teacherName, new TeacherLoad(teacherName));
        Discipline discipline1 = new Discipline("Математика", 2, 32, 16, 16, 0);
        teachers[teacherName].AddDiscipline(discipline1);
        Discipline discipline2 = new Discipline("Алгебра", 1, 32, 24, 16, 1);
        teachers[teacherName].AddDiscipline(discipline2);
        Discipline discipline3 = new Discipline("Философия", 3, 71, 8, 8, 1);
        teachers[teacherName].AddDiscipline(discipline3);
        Discipline discipline4 = new Discipline("Основы программирования", 4, 12, 32, 16, 0);
        teachers[teacherName].AddDiscipline(discipline4);
        Discipline discipline5 = new Discipline("Чебурек", 5, 43, 2, 16, 0);
        teachers[teacherName].AddDiscipline(discipline5);

        while (true)
        {
            //Меню пользователя
            Console.WriteLine("1 - Ввести дисциплину");
            Console.WriteLine("2 - Удалить дисциплину");
            Console.WriteLine("3 - Поиск дисциплины");
            Console.WriteLine("4 - Вывести все дисциплины");
            Console.WriteLine("5 - Завершение работы\n");

            int choice = GetChoice(1, 5); //Выбранная операция

            switch (choice)
            {
                case 1: //Добавление дисциплины в дерево
                    if (teachers.Count == 0)
                    {
                        Console.WriteLine("\nСписок преподавателей пуст.\nДобавьте хотя бы одного преподавателя\n");
                        break;
                    }

                    Console.Write("\nВведите наименование дисциплины: ");
                    string name = GetString();
                    Console.Write("Введите семестр: ");
                    int semester = GetInt();
                    Console.Write("Введите количество студентов: ");
                    int students = GetInt();
                    Console.Write("Введите количество лекционных часов: ");
                    int lectureHours = GetInt();
                    Console.Write("Введите количество практических часов: ");
                    int practiceHours = GetInt();
                    Console.Write("Тип контроля (0 - экзамен, 1 - зачёт). ");
                    int controlType = GetChoice(0, 1);

                    Discipline discipline = new Discipline(name, semester, students, lectureHours, practiceHours, controlType);
                    teachers[teacherName].AddDiscipline(discipline);

                    Console.WriteLine("\nДисциплина \"" + name + "\" преподавателя \"" + teacherName + "\" добавлена.\n");
                    break;

                case 2: //Удаление дисциплины из дерева
                    if (teachers.Count == 0)
                    {
                        Console.WriteLine("\nСписок преподавателей пуст.\nДобавьте хотя бы одного преподавателя\n");
                        break;
                    }

                    Console.Write("\nВведите наименование дисциплины: ");
                    string removeName = GetString();

                    teachers[teacherName].DeleteDiscipline(removeName);

                    Console.WriteLine("\nДисциплина \"" + removeName + "\" преподавателя \"" + teacherName + "\" удалена.\n");
                    break;

                case 3: //Поиск дисциплины в дереве
                    if (teachers.Count == 0)
                    {
                        Console.WriteLine("\nСписок преподавателей пуст.\nДобавьте хотя бы одного преподавателя\n");
                        break;
                    }

                    Console.Write("\nВведите наименование дисциплины: ");
                    string printName = GetString();

                    teachers[teacherName].SearchByName(printName);
                    break;

                case 4: //Вывод всех дисциплин
                    if (teachers.Count == 0)
                    {
                        Console.WriteLine("\nСписок преподавателей пуст.\nДобавьте хотя бы одного преподавателя\n");
                        break;
                    }

                    teachers[teacherName].PrintAll();
                    break;

                case 5: //Завершение работы программы
                    return;
            }
        }
    }

    //Функция проверки корректности выбранной операции
    //Аргументы: диапазон допустимых значений
    //Возвращает выбранное значение
    static int GetChoice(int min, int max)
    {
        int choice = 0;
        bool isCorrect = false;

        while (!isCorrect)
        {
            Console.Write("Выберите операцию ({0}-{1}): ", min, max);

            isCorrect = int.TryParse(Console.ReadLine(), out choice);

            if (isCorrect && (choice < min || choice > max))
            {
                isCorrect = false;
            }

            if (!isCorrect)
            {
                Console.WriteLine("Вы ввели некорректное значение. Попробуйте снова.\n");
            }
        }

        return choice;
    }

    //Функция проверки корректности ввода целого натурального числа
    //Возвращает введённоe число
    static int GetInt()
    {
        int number = 0;
        bool isCorrect = false;

        while (!isCorrect)
        {
            isCorrect = int.TryParse(Console.ReadLine(), out number);

            if (!isCorrect || number <= 0)
            {
                Console.WriteLine("Вы ввели некорректное значение. Попробуйте снова.\n");
            }
        }

        return number;
    }

    //Функция проверки корректности ввода строки (ненулевой)
    //Возвращает введённую строку
    static string GetString()
    {
        string name = Console.ReadLine();
        bool isCorrect = true;

        while (isCorrect)
        {
            isCorrect = string.IsNullOrEmpty(name);

            if (isCorrect)
            {
                Console.WriteLine("\nВы ничего не ввели. Попробуйте снова\n");
            }
            else
            {
                isCorrect = false;
            }
        }

        return name;
    }
}