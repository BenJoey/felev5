#include <fstream>
#include <vector>
#include <future>
#include <iostream>

std::vector<std::vector<int>> calculate_solution(const std::vector<std::vector<int>>& data)
{
    //std::vector<std::vector<int>> ToRet(data.size(), std::vector<int>(0));
    return data;
}

std::string Conv_Vect_To_String(const std::vector<std::vector<int>>& data)
{
    std::string ToRet = "";
    for(int i = 0;i<data.size();i++)
    {
        for(int j = 0;i<data[j].size();j++)
        {
            //std::cout << data[i][j] <<std::endl;
            ToRet += data[i][j];
        }
        ToRet += "\r\n";
    }
    std::cout << ToRet << std::endl;
    return ToRet;
}
    
int main()
{
    std::ifstream input("input.txt");
	
	unsigned int N;
	
	input >> N;
    
	std::vector<std::vector<std::vector<int>>> data(N, std::vector<std::vector<int>>(0));
    //data.resize(N);
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
    //std::cout << data[2][1][0];
    input.close();
    std::vector<std::future<std::vector<std::vector<int>>>> results;
    for(size_t i=0;i<N;i++)
    {
        results.push_back(std::async(std::launch::async, calculate_solution, data[i]));
    }
    std::ofstream output("output.txt");
    for(std::future<std::vector<std::vector<int>>>& f : results)
    {
        f.wait();
        //std::string Out = Conv_Vect_To_String(f.get());
        //output << Out <<std::endl;
        std::vector<std::vector<int>> Res = f.get();
        for(int i = 0;i<Res.size();i++)
        {
            for(int j = 0;i<Res[j].size();j++)
            {
                //std::cout << data[i][j] <<std::endl;
                output << Res[i][j];
            }
            output << std::endl;
        }
    }
    output.close();
    return 0;
}