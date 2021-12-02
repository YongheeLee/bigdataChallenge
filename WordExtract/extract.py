from konlpy.tag import Hannanum
from collections import Counter
import os
from os import walk

hannanum = Hannanum()

str = "D:/공모전/채용정보빅데이터챌린지/datasetFiles/text"

f = []
for (dirpath, dirnames, filenames) in walk(str):
    for i in range(0, len(filenames)):
        full = os.path.join(dirpath, filenames[i])
        f = open(full, "r", encoding='UTF8') 
        lines = f.read()
        x = hannanum.nouns(lines)
        newFUll = full+".txt"
        fw = open(newFUll, "w", encoding='UTF8')
        for w in range(0, len(x)):
            fw.write(x[w])
            fw.write("\n")
        fw.close()
