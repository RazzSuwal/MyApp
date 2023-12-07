using MyProject.Models;

public static class SortingHelper
{
    public static void BubbleSortByName(List<Category> categories)
    {
        int n = categories.Count;
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if (string.Compare(categories[i - 1].Name, categories[i].Name) > 0)
                {
                    // Swap categories[i-1] and categories[i]
                    Category temp = categories[i - 1];
                    categories[i - 1] = categories[i];
                    categories[i] = temp;
                    swapped = true;
                }
            }
            n--; // Decrease the size of the unsorted portion
        } while (swapped);
    }

    public static bool BinarySearchByName(List<Category> categories, string categoryName)
    {
        int left = 0;
        int right = categories.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int comparisonResult = string.Compare(categories[mid].Name, categoryName);

            if (comparisonResult == 0)
            {
                // Category with the same name already exists
                return true;
            }

            if (comparisonResult < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        // Category with the given name does not exist
        return false;
    }
}
