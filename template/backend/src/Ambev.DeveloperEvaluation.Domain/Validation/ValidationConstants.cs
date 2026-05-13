namespace Ambev.DeveloperEvaluation.Domain.Validation;

public static class ValidationConstants
{
    public const int MaxCustomerNameLength = 200;
    public const int MaxBranchNameLength = 100;
    public const int MaxProductNameLength = 200;
    public const int MaxItemsPerSale = 50;
    
    public const decimal MinDiscount = 0;
    public const decimal MaxDiscount = 100;
    
    public const int MinQuantity = 1;
    public const int MaxQuantity = 20;
}