#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;


template<typename T>
class SmartPointer {
public:
    SmartPointer(T* ptr) {
        this->ptr = ptr;
    }
    ~SmartPointer() {
        delete ptr;
    }
    T& operator*() {
        return *ptr;
    }
private:
    T* ptr;
};

template <typename T, typename N>
T sum1(T a, N b) {
    T n = a + b;
    return n;
}

int main()
{
    vector <int> V = { 1,2,4,5,12,4,1435 };
    sort(V.begin(), V.end());
    int c = count_if(V.begin(), V.end(), [](auto m) {return m > 5; });
    cout << c << " ";
    
}

