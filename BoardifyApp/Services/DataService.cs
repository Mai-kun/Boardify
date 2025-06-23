using BoardifyApp.Models;

namespace BoardifyApp.Services;

public class DataService
{
    public List<Board> LoadBoards()
    {
        // позже заменить на загрузку из БД
        return
        [
            new Board
            {
                Name = "Личное",
                Columns =
                [
                    new Column
                    {
                        Title = "To Do",
                        Tasks = 
                        [
                            new TaskCard { Title = "Задача 1Задача 1Задача 1Задача 1Задача 1Задача 1Задача 1Задача 1Задача 1Задача 1" },
                        ]
                    },
                    new Column { Title = "In Progress" },
                    new Column { Title = "Done" }
                ]
            }
        ];
    }
}