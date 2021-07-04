#%%
import select
import socket
import threading
import logging
from time import sleep 

class Server:
	def __init__ (self, port):
		self.thread = None
		self.port = int(port)

	def start(self):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

		
		self.thread = threading.Thread(target=self.run)
		self.thread.daemon = True
		self.thread.start()

	def run(self):
		for _ in range(10):
			print("Hello from thread function")
	
	def stop(self):
		pass

	def onConnect(self):
		pass

	def onDisconnect(self):
		pass

	def onMsgReceive(self):
		pass

	def sendMsg(self):
		pass



if __name__ == '__main__':
	port = 1234
	PServer = Server(port)
	PServer.start()

# %%
