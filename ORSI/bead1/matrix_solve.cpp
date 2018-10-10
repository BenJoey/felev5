#include <fstream>
#include <vector>
#include <future>
#include <iostream>
//#include <algorithm>

std::vector<std::vector<int>> calculate_solution(const std::vector<std::vector<int>>& ToCalc)
{
    std::vector<std::vector<int>> ToRet(ToCalc);
    for(int i = 0; i < ToRet.size(); i++)
    {
        for(int j = 0; j < ToRet.size(); j++)
        {
            if(ToRet[i][j] != 0 ){}
        }
    }
    return ToCalc;
}
    
int main()
{
    std::ifstream input("input.txt");
	
	unsigned int N;
	
	input >> N;
    
	std::vector<std::vector<std::vector<int>>> data(N);
    for(int i = 0; i < N; i++)
    {
        int Size;
        input >> Size;
        data[i].resize(Size);
        for(int j = 0; j < Size; j++)
        {
            data[i][j].resize(Size);
            for(int k =0; k < Size; k++)
            {
                input >> data[i][j][k];
            }
        }
    }
    input.close();
    std::vector<std::future<std::vector<std::vector<int>>>> results;
    for(size_t i = 0; i < N; i++)
    {
        results.push_back(std::async(std::launch::async, calculate_solution, data[i]));
    }
    std::ofstream output("output.txt");
    for(std::future<std::vector<std::vector<int>>>& f : results)
    {
        f.wait();
        std::vector<std::vector<int>> Res = f.get();
        for(int i = 0; i < Res.size(); i++)
        {
            for(int j = 0; j < Res[i].size(); j++)
            {
                output << Res[i][j];
                if(j != Res[i].size() - 1)output << " ";
            }
            output << std::endl;
        }
    }
    output.close();
    return 0;
}