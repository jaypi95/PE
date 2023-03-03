import matplotlib.pyplot as plt
import numpy as np


def getCsvData(aufgabe):
    csvData = np.genfromtxt(f'lab1/time_series_aufgabe{aufgabe}.csv', delimiter=',', dtype=None, skip_header=0, encoding='utf-8')
    return [{'label': csvData[0, i], 'data': csvData[1:, i].astype(np.float64)} for i in range(0, csvData.shape[1])]

def plotDataOfCsv(plotData, forceAxis='x'):
    for i in range(1, len(plotData)):
        plt.plot(plotData[0]['data'], plotData[i]['data'], label=plotData[i]['label'])

    plt.xlabel('t [s]')
    plt.ylabel('x [m]')
    plt.title(f'Time series of moving object {forceAxis}-axis')
    plt.yscale('symlog')
    plt.legend()
    plt.grid()
    # for datatype in ['png', 'pdf', 'svg', 'eps', 'ps', 'raw', 'rgba', 'svgz', 'tif', 'tiff', 'jpg', 'jpeg', 'pgf', 'bmp', 'gif']:
    for datatype in ['png', 'svg']:
        plt.savefig(f'time_series_{forceAxis}.{datatype}')

    plt.show()

plotDataOfCsv(getCsvData('1'), 'x')
plotDataOfCsv(getCsvData('2'), 'z')