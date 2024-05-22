namespace Infrastructure.Search.Pagination;

public record Paging
{
    private const ushort UpperSize = 100;
    private const ushort DefaultSize = 10;
    private const ushort DefaultNumber = 1;
    private const ushort Zero = 0;

    public Paging(ushort number = DefaultNumber, ushort size = DefaultSize)
    {
        Size = size switch
        {
            Zero => DefaultSize,
            > UpperSize => UpperSize,
            _ => size
        };

        Number = number is Zero
            ? DefaultNumber
            : number;
    }

    public ushort Size { get; }
    public ushort Number { get; }
}