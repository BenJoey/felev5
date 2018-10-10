#include <fstream>
#include <vector>
#include <future>

int main()
{
    std::ifstream input("input.txt");
	
	unsigned int N;
	
	input >> N;
    
	std::vector<std::vector<std::vector<int>>> data(N);
    for(int i = 0; i<N;i++)
    {
        int Size;
        input >> Size;
        data[i].resize(Size);
        for(int j = 0;j<Size;j++)
        {
            data[i][j].resize(Size);
            for(int k =0;k<Size;k++)
            {
                input >> data[i][j][k];
            }
        }
    }
    input.close();
    return 0;
}