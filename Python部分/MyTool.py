import socket
import sys
import numpy as np

class Py2Unity:
    def __init__(self):
        self.__conn = self.__Connect()

    def SendToUnity(self, data):
        #print(data)
        self.__conn.sendall(bytes(str(data), encoding='utf-8'))

    def RecFromUnity(self):
        data = str(self.__conn.recv(1024))
        data = data.replace("b'", "").replace("'", "")
        #print(data)
        if data == "exit":
            sys.exit()
        return data

    def __Connect(self):
        s = socket.socket()
        s.bind(("127.0.0.1", 7758))
        s.listen(1)
        print("wait for connect...")
        return s.accept()[0]

def read_store(path):
    f = open(path)
    lst = f.read().splitlines()
    f.close()
    res_lst = []
    for i in range(len(lst)):
        temp = lst[i].split(",")
        tem_lst = []
        for j in range(len(temp)):
            tem_lst.append(float(temp[j]))
        res_lst.append(tem_lst)
    return np.array(res_lst)

def save_weights(weights, path):
    data = ""
    for i in range(len(weights)):
        data = data + str(weights[i])
        if (i != len(weights) - 1):
            data = data + ","
    fh = open(path, 'w', encoding='utf-8')
    fh.write(data)
    fh.close()

def get_net_weights(net):
    a = net.state_dict()
    cell_lst = []
    weights = []
    n = 0
    for k in a:
        size = len(a[k].shape)
        if (size != 1):
            for c in a[k].data.numpy():
                cell_lst.append(c.tolist())
        else:
            for p in a[k].data.numpy():
                cell_lst[n].append(p)
                n += 1
    for c in cell_lst:
        for w in c:
            weights.append(w)
    return weights
