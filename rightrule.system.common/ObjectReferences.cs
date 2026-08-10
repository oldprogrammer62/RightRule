namespace RightRule.System.Common;

public class ObjectReferences
{
    private readonly List<ObjectReference> _objects;
    public List<ObjectReference> Objects => _objects;

    public ObjectReferences()
    {
        _objects = new List<ObjectReference>();
    }

    public ObjectReferences(List<ObjectReference> objects)
    {
        _objects = objects ?? new List<ObjectReference>();
    }

    public void Add(string name, object value)
    {
        _objects.Add(new ObjectReference(name, value));
    }
}

public class ObjectReference
{
    public string Name { get; set; }
    public object Value { get; set; }
    public ObjectReference(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
