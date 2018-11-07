#include <fstream>
#include <vector>
#include <sstream>
#include <iostream>
#include <string>
#include <future>

#include "build.h"

Build * get_Dominant_Elem(std::vector<Build> Arr){
    if(Arr.size() == 1){
        return &(Arr[0]);
    }
    int lsize = Arr.size() / 2;
    int count = 0;
    
    std::vector<Build> leftArr(Arr.begin(), Arr.begin() + lsize);
    std::future<Build*> leftResult = std::async(std::launch::async, get_Dominant_Elem, leftArr);
    
    std::vector<Build> rightArr(Arr.begin() + lsize, Arr.end());
    std::future<Build*> rightResult = std::async(std::launch::async, get_Dominant_Elem, rightArr);
    
    leftResult.wait();    
    Build* leftDom = leftResult.get();
    if(leftDom != nullptr){
        for(int i = 0; i < Arr.size(); ++i){
            if((*leftDom) == Arr[i])count++;
        }
        std::cout<<count<<std::endl;
        if(count > lsize) return leftDom;
    }
    count = 0;
    rightResult.wait();
    Build* rightDom = rightResult.get();
    if(rightDom != nullptr){
        for(int i = 0; i < Arr.size(); ++i){
            if((*rightDom) == Arr[i])count++;
        }
        std::cout<<count<<std::endl;
        if(count > lsize) return rightDom;
    }
    return nullptr;
}

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
    }
    /*std::vector<Build> leftArr(data.begin(), data.begin() + 1);
    std::cout<<"LEFTARR:\n";
    for(auto it = leftArr.begin(); it<leftArr.end();++it){
        std::cout<<(*it);
    }
    std::vector<Build> rArr(data.begin()+1, data.end());
    std::cout<<"RARR:\n";
    for(auto it = rArr.begin(); it<rArr.end();++it){
        std::cout<<(*it);
    }
    /*
    Build* Dom = &(data[2]);
    Build* Dom2 = &(data[0]);
    if((*Dom) == (*Dom2)){
    std::cout<<(*Dom);}
    else{
    std::cout<<"HIBA\n";}*//*
    std::future<Build*> Get_Dom_Build = std::async(std::launch::async, get_Dominant_Elem, data);
    Get_Dom_Build.wait();*/
    Build* DomBuild = get_Dominant_Elem(data);
    if(DomBuild == nullptr){
        std::cout<<"ERR"<<std::endl;
    }
    else{
        std::cout<<(*DomBuild);
    }
}