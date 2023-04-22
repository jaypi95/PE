from math import ceil, floor

import matplotlib.pyplot as plt
import numpy as np

def isNaN(num):
    return num != num

def _vector_check(u, dtype=None):
    """after _validate_vector from scipy.spatial.distance
    : Requires: list/array arrays need to be 1D
    : Returns:  proper 1D vector
    : Notes Ensure values such as u=1 and u=[1] still return 1-D arrays.
    """
    u = np.asarray(u, dtype=np.float64, order='c').squeeze()
    u = np.atleast_1d(u)
    if u.ndim > 1:
        raise ValueError("Input vector should be 1-D.")
    return u

def cos_3pnt(a,b,c):
    """cosine between 3 points
    : Requires  3 points, the middle one is assumed to form the start of the vectors
    : Output    the angle between bac
    : Notes    This form facilitates determining angles as one progresses along a line
    """
    a, b, c = [_vector_check(i) for i in [a,b,c] ]
    ba = a - b
    bc = c - b
    cos_a = np.dot(ba, bc) / (np.linalg.norm(ba) * np.linalg.norm(bc))
    angle = np.arccos(cos_a)
    if np.allclose(angle,np.pi):
        return np.pi, np.degrees(np.pi)
    if np.allclose(angle,0.0):
        return 0.0, 0.0
    return angle, np.degrees(angle)


def get_bends(data):
    x = data[0]["data"]
    y = data[1]["data"]
    d = np.array(list(zip(x, y)))

    ang = np.array([cos_3pnt(d[i - 1], d[i], d[i + 1])[1] for i in range(1, len(d) - 1)])

    # take the index where the angle is not 180
    bends = []
    for i in range(len(ang)):
        if isNaN(ang[i]):
            continue
        if int(ang[i]) != 180 and int(ang[i]) != 179 and int(ang[i]) != 181:
            bends.append(i)

    # print the angle values at the bends
    print(f"Bends detected at array index = {bends}")
    print(f"Angle values at bends = {ang[bends]}")
    return bends


def getCsvData(pathToFile):
    csvData = np.genfromtxt(pathToFile, delimiter=',', dtype=None, skip_header=0, encoding='utf-8')
    return [{'label': csvData[0, i], 'data': csvData[1:, i].astype(np.float64)} for i in range(0, csvData.shape[1])]

def plotDataOfCsv(plotData, pathToFile):
    for i in range(1, len(plotData)):
        plt.plot(plotData[0]['data'], plotData[i]['data'], label=plotData[i]['label'])

    # plot a red point at the bend
    # bends = get_bends(plotData)
    # for i in range(len(bends)):
    #     plt.plot(plotData[0]["data"][bends[i] + 1], plotData[1]["data"][bends[i] + 1], 'ro')
    #     """plt.text(plotData[0]["data"][bends[i] + 1] + .075,
    #              plotData[1]["data"][bends[i] + 1] + .075,
    #              i, fontsize=9)"""

    title = pathToFile.split('_')
    typeOfAxis = title[3].split('.')[0]
    axisName = f"{getUnitOfThingAndStuff(title[3])}"
    plt.xlabel("t [s]")
    plt.ylabel(f'{typeOfAxis} [{axisName}]')
    plt.title(f'Time series of moving {title[2]} {typeOfAxis}')
    plt.legend()

    plt.grid()
    # for datatype in ['png', 'pdf', 'svg', 'eps', 'ps', 'raw', 'rgba', 'svgz', 'tif', 'tiff', 'jpg', 'jpeg', 'pgf', 'bmp', 'gif']:
    for datatype in ['png', 'svg']:
        plt.savefig(f'{pathToFile}.{datatype}')

    plt.show()

def getUnitOfThingAndStuff(type):
    if type == 'impulse':
        return 'm*v'
    elif type == 'position':
        return 'm'
    elif type == 'velocity':
        return 'm/s'
    else:
        return 'no unit'


files = ['time_series_Wuerfel1_impulse_x.csv', 'time_series_Wuerfel1_position_x.csv',
         'time_series_Wuerfel1_velocity_x.csv', 'time_series_Wuerfel2_impulse_x.csv',
         'time_series_Wuerfel2_position_x.csv', 'time_series_Wuerfel2_velocity_x.csv',
         'time_series_Impulse both cubes_impulse_x.csv']
for file in files:
    file = "csv/" + file
    plotDataOfCsv(getCsvData(file), file)

