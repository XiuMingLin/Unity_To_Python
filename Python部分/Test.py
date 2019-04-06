import MyNet
import MyTool

p2u = MyTool.Py2Unity()
net = MyNet.Net([1,4,1])
ws = MyTool.get_net_weights(net)
MyTool.save_weights(ws,"C:\\Users\\XiuMing\\Desktop\\weights.txt")
p2u.SendToUnity("save_ok")
while 1:
    p2u.RecFromUnity()
    store = MyTool.read_store("C:\\Users\\XiuMing\\Desktop\\store.txt")
    net.learn(store)
    ws = MyTool.get_net_weights(net)
    MyTool.save_weights(ws, "C:\\Users\\XiuMing\\Desktop\\weights.txt")
    p2u.SendToUnity("save_ok")
