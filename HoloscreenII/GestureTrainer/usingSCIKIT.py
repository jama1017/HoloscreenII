from sklearn.cluster import  KMeans
import numpy as np
#addition pkg: scipy

fname = "handData_G"
for i in range(3):
    cur_fname = fname + str(i) + '.txt'
    npData = np.zeros(shape=(1,15))
    with open(cur_fname) as f:
        readData = f.read()                                                                                                                 
        readData.replace('\n',',')
        #readData = ','.join(readData)
        print (readData )
        #temp = np.asarray(np.float,dtype=np.float32)
