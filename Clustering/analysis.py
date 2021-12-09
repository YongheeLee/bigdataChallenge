import seaborn as sns
import pandas as pd
import matplotlib.pyplot as plt

data = pd.read_csv('D:\공모전\채용정보빅데이터챌린지\datasetFiles\jobsal.csv')
#data = data.values

iris = sns.load_dataset("tips")    # 붓꽃 데이터
ddd =iris.values

sns.boxplot(x="stack", y="salary",hue="minmax",
             palette=["m", "g"],
            data=data)
sns.despine(offset=10, trim=True)
plt.show()