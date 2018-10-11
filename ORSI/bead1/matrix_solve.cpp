#include <fstream>
#include <vector>
#include <future>

std::pair<int,int> get_empty_field(const std::vector<std::vector<int>>& ToSearch)
{
	for(int i = 0; i < ToSearch.size(); i++)
		for(int j = 0; j < ToSearch.size(); j++)
			if(ToSearch[i][j] == 0)
				return std::make_pair(i,j);
	return std::make_pair(-1,-1);
}

bool Can_It_Go_Here(const std::vector<std::vector<int>>& ToSearch, int Row, int Column, int ToValidate)
{
	for(int i = 0; i < ToSearch.size(); i++)
	{
		if(i != Row && ToSearch[i][Column] == ToValidate) return false;
		if(i != Column && ToSearch[Row][i] == ToValidate) return false;
	}
	return true;
}

//Recursive function to solve the "sudoku"-like task with the backtracking algorithm
bool solve(std::vector<std::vector<int>>& ToRet)
{
	if(std::make_pair(-1,-1) == get_empty_field(ToRet)) return true;

	std::pair<int,int> current = get_empty_field(ToRet);  // First item is the row number & second one is the column

	for(int num = 1; num <= ToRet.size(); num++)
		if(Can_It_Go_Here(ToRet, current.first, current.second, num))
		{
			ToRet[current.first][current.second] = num;
			if(solve(ToRet)) return true;
			ToRet[current.first][current.second] = 0;  //If a field could not be filled later, than we reset this field
		}
	return false;
}

std::vector<std::vector<int>> get_answer(const std::vector<std::vector<int>>& Original)
{
	std::vector<std::vector<int>> ToRet(Original);
	if(solve(ToRet)) return ToRet;
	else return Original;
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
		results.push_back(std::async(std::launch::async, get_answer, data[i]));
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
