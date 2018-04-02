from sklearn.cluster import  KMeans
import numpy as np
from sklearn.neighbors import KNeighborsClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.cross_validation import train_test_split
import time

#addition pkg: scipy

# Read training data
data_fname = "handDataG"
all_npData = np.empty((0,15),float)
all_label = np.empty((1,0), int)
#print(all_label.shape)
#all_npData = np.array(all_npData)
for i in range(4):
    cur_fname = data_fname + str(i) + '_new.txt'
    npData = None
    with open(cur_fname) as f:
        readData = f.read()        
        readData = readData.replace("\n",",")
        npData = np.fromstring(readData, dtype=np.float,sep=',')
        npData = np.reshape(npData, (-1,15))
        all_npData = np.concatenate((all_npData, npData),axis=0)
        label = np.zeros((1, npData.shape[0]))
        print(npData.shape)
        label = label + i
        all_label = np.concatenate((all_label,label),axis=1)

all_label = all_label.flatten()
#all_npData = np.transpose(all_npData)
print(all_npData.shape)
print(all_label.shape)

# Read testing data
test_fname = "testing_2_0_1_quantou_ba_2.txt"
with open(test_fname) as f:
  readData = f.read()        
  readData = readData.replace("\n",",")
  rdata = np.fromstring(readData, dtype=np.float,sep=',')
  rdata = np.reshape(rdata, (-1,15))
#print(rdata)


# instantiate a logistic regression model, and fit with X and y
X_train, X_test, y_train, y_test = train_test_split(all_npData, all_label, test_size=0.2, random_state=2)
#model = LogisticRegression(class_weight={0:, 1: ,2: ,3: ,4:, 5: ,6: ,7: ,8: ,9: ,10:, 11})
model = LogisticRegression()
#model = KNeighborsClassifier()
#model = DecisionTreeClassifier()
model = model.fit(X_train, y_train)

# check the trainning/testing err on the training set
print(model.score(X_train, y_train))
print(model.score(X_test, y_test))

# test the prediction time
start = time.time()
model.predict(X_train)
end = time.time()
print("%1.11f"%(float(end - start)))
print("%1.11f"%(start))
print("%1.11f"%(end))
#temp = np.asarray(np.float,dtype=np.float32)
