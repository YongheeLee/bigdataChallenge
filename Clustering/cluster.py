import numpy as np
import pandas as pd
from kmodes.kmodes import KModes
from sklearn.cluster import KMeans
from sklearn.cluster import DBSCAN
from sklearn.cluster import SpectralClustering

#import csv
data = pd.read_csv('D:\공모전\채용정보빅데이터챌린지\datasetFiles\jobOnehot.csv', header=None)
data = data.values

onehot = data[:,1:202]

table = data[:,0].reshape(-1,1)

for i in range(5, 10):
    kmeans = KMeans(n_clusters=i)
    kmeans.fit(onehot)

    lables = kmeans.labels_
    table = np.hstack([table, lables.reshape(-1,1)])


#extract index and data

df = pd.DataFrame(table)

file = f'D:\공모전\채용정보빅데이터챌린지\datasetFiles\job_kmean.csv'

df.to_csv(file)

print("KMeans Finished")
#fit 4 models

dbscan = DBSCAN(min_samples=3)
dbscan.fit(onehot)

table = data[:,0].reshape(-1,1)
table = np.hstack([table, dbscan.labels_.reshape(-1,1)])
df = pd.DataFrame(table)

file = f'D:\공모전\채용정보빅데이터챌린지\datasetFiles\job_dbscan.csv'
df.to_csv(file)
print("DBSCAN Finished")

table = data[:,0].reshape(-1,1)
for i in range(5, 10):
    spectra = SpectralClustering(n_clusters=i)
    spectra.fit(onehot)

    lables = spectra.labels_
    table = np.hstack([table, lables.reshape(-1,1)])

df = pd.DataFrame(table)

file = f'D:\공모전\채용정보빅데이터챌린지\datasetFiles\job_SpectralClustering.csv'

df.to_csv(file)
print("SpectralClustering Finished")


table = data[:,0].reshape(-1,1)
for i in range(5, 10):
    kmode = KModes(n_clusters=i)
    kmode.fit(onehot)
    lables = kmode.labels_
    table = np.hstack([table, lables.reshape(-1,1)])
df = pd.DataFrame(table)

file = f'D:\공모전\채용정보빅데이터챌린지\datasetFiles\job_kmode.csv'

df.to_csv(file)
print("KModes Finished")
