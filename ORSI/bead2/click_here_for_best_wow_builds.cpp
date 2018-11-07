#include <fstream>
#include <vector>
#include <sstream>
#include <iostream>
#include <string>

#include "build.h"

int main()
{
	std::ifstream input("input.txt");
	unsigned int N;
	input >> N;
    std::vector<build> data;
    std::string s;
    while(getline(input, s)){
        //if(!getline(input, s)) break;
        std::istringstream ss(s);
        std::vector<std::string> Line;
        while(getline(ss, s, ';')){
            //std::string sec;
            //if(!getline(ss, sec, ';')) break;
            Line.push_back(s);
        }
        std::cout<<"TEST"<<std::endl;
        build temp(Line[0], Line[1], std::stoi(Line[2]), Line[3]);
        data.push_back(temp);
        std::cout<<temp<<std::endl;
    }
}