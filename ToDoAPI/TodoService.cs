namespace ToDoAPI;

public class TodoService
{
    private readonly List<TodoItem> _items = new();

    public IEnumerable<TodoItem> GetAll()
    {
        //returns the list of all todo items    
        return _items;
    }

    public TodoItem? GetById(int id)
    {
        //returns the first item in the list that matches the specified id, or null if no such item is found
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public void Add(TodoItem item)
    {
        // item.Id = _items.Count > 0 ? _items.Max(x => x.Id) + 1 : 1;
        // _items.Add(item);

        // The above code is a more concise way to assign the Id, but it may be less readable for some developers. The following code is more explicit and easier to understand.

        //checking if the list is empty, if it is then assign the Id to 1,
        if (_items.Count == 0)
        {
            item.Id = 1;
        }

        //otherwise assign the Id to the maximum Id in the list plus 1
        else
        {
            item.Id = _items.Max(x => x.Id) + 1;
        }
        //adding the item to the list
        _items.Add(item);
    }

    public void Update(TodoItem item)
    {
        // Find the index of the item with the same Id as the provided item
        var index = _items.FindIndex(x => x.Id == item.Id);

        //if found then update the item at that index with the provided item
        if (index != -1)
        {
            _items[index] = item;
        }
    }

    public void Delete(int id)
    {
        // Find the index of the item with the specified Id
        var index = _items.FindIndex(x => x.Id == id);

        //if found then remove the item at that index from the list
        if (index != -1)
        {
            _items.RemoveAt(index);
        }
    }
}