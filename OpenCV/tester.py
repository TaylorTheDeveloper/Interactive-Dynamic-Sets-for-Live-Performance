#Provides a simple interface for streaming Video Data to unity.
#
#Simply run like: test.py data/<filename> 
#                 and it will stream the data to unity 
#                 in a loop via localhost port 5005 so 
#                 you wont need to open a video stream

import sys
import socket

def sendUDP(mess):
        '''message with id at begining'''
        print "sent ", mess, " to ", UDP_IP,":",UDP_PORT
        sock.sendto(mess , (UDP_IP, UDP_PORT))

UDP_IP = "127.0.0.1"
UDP_PORT = 5005


sock = socket.socket(socket.AF_INET, # Internet
                 socket.SOCK_DGRAM) # UDP

fo = open("f1","r")
lines = fo.readlines()

while (True):
	for l in lines:
		sendUDP(l)
