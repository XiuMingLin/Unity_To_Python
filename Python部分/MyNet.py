import torch
from torch.autograd import Variable
import torch.nn as nn


class Net(nn.Module):
    def __init__(self,shape):
        super(Net, self).__init__()
        self.shape = shape
        self.cn = nn.Sequential()
        for i in range(len(shape) - 1):
            layer = nn.Linear(shape[i],shape[i+1])
            layer.weight.data.normal_(0,0.1)
            self.cn.add_module(str(i),layer)
        self.mls = nn.MSELoss()
        self.opt = torch.optim.Adam(self.parameters(), lr=1e-2)
    def forward(self, inputs):
        for i in range(len(self.shape)-1):
            inputs = self.cn.__getitem__(i)(inputs)
            inputs = torch.tanh(inputs)
        return inputs
    def learn(self,store):
        for i in range(10):
            x = torch.FloatTensor(store[:,:1])
            y = torch.FloatTensor(store[:,1:2])
            dy = self.forward(x)
            loss = self.mls(dy,y)
            print(loss)
            self.opt.zero_grad()
            loss.backward()
            self.opt.step()