from sklearn.cluster import  KMeans
import numpy as np
#addition pkg: scipy

fname = "handData_G"
for i in range(3):
    cur_fname = fname + str(i) + '.txt'
    npData = None
    with open(cur_fname) as f:
        readData = f.read()        
        print(type(readData))                                                                                                         
        readData = readData.replace("\n",",")
        #readData = ','.join(readData)
        npData = np.fromstring(readData, dtype=np.float,sep=',')
        npData = np.reshape(npData, (15,-1))
        npData = npData.astype(np.float)
        print(npData)
        #temp = np.asarray(np.float,dtype=np.float32)
