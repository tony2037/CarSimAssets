import numpy as np
import cv2
import sys, os
sys.path.append('D:\\SelfDriving\\semantic-segmentation\\utils') # To extern the path
from preprocess import onehot

pixel2label = [
        [142, 0, 0],     # car
        [128, 64, 128],  # road
        [180, 130, 70],   # sky
        [160, 170, 250], # parking
        [0, 0, 0], # Self
        [128, 128, 128], # obstacle
    ]

EuclideanDistances = [0]* len(pixel2label)

path = './sim_labels/'

def calculate_distance(a, b):
    s = 0
    for i in range(0, 3):
        s += (a[i] - b[i])** 2
    return s

def revise(BGR):
    for i in range(0, len(pixel2label)):
        EuclideanDistances[i] = calculate_distance(BGR, pixel2label[i])
    BGR = pixel2label[EuclideanDistances.index(min(EuclideanDistances))]
    BGR = np.uint8(BGR)
    return BGR

if __name__ == '__main__':
    print(EuclideanDistances)
    number = int(input('data number'))
    for i in range(0, number):
        print('Now dealing with %s' % str(i))
        image = cv2.imread('%s.jpg' % str(i))
        for w in range(0, image.shape[0]):
            for h in range(0, image.shape[1]):
                image[w][h] = revise(image[w][h])
        cv2.imwrite('%s%s.png' % (path, str(i)), image)

    for i in range(0, number):
        print('Now dealing with %s' % str(i))
        image = cv2.imread('%s%s.png' % (path, str(i)))
        onehot(image, path, i)