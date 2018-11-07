#include <fstream>
#include <vector>
#include <sstream>
#include <iostream>
#include <string>

#include "build.h"

int main()
{
	std::ifstream input("input.txt");
    std::vector<Build> data;
    std::string s;
    getline(input, s);
    unsigned int N = std::stoi(s);
    while(getline(input, s)){
        std::istringstream ss(s);
        std::vector<std::string> Line;
        while(getline(ss, s, ';')){
            Line.push_back(s);
        }
        Build temp(Line[0], Line[1], std::stoi(Line[2]), Line[3]);
        data.push_back(temp);
        std::cout<<temp;
    }
}