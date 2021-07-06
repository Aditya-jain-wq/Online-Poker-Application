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
		
		try:
			self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEPORT, 1)
		except Exception as e:
			print(f"Socket exception on SO_REUSEPORT {str(e)}")
			return False
		
		print(f"Binding to port {self.port}")
		try:
			self.sock.bind(('', self.port))
		except socket.error as e:
			print(f"Error while binding {str(e)}")
			return False

		try:
			self.sock.setblocking(False)
		except socket.error as e:
			print(f"failed to set non-blocking {e}")
			return False

		try:
			self.sock.listen(3)
		except socket.error as e:
			print(f"Failed to listen with error {str(e)}")
			return False

		self.thread = threading.Thread(target=self.run)
		self.thread.daemon = True
		self.thread.start()
		return True
	
	def stop(self):
		try:
			self.sock.shutdown(socket.SHUT_RDWR)
		except socket.error as e:
			print(f"Failed to shutdown server with error {str(e)}")
		

	def run(self):
		inputs = [self.sock]
		outputs = []
		while inputs:
			readable, _, excetional = select.select(inputs, outputs, inputs)

			for sock in readable:
				if sock in self.sock: # new client
					client_sock, client_address = self.sock.accept()
					try:
						client_sock.setblocking(False)
					except socket.error as e:
						print(f"Failed to set client socket to non-blocking address:{client_address}, error:{str(e)}")
						return False
					inputs.append(client_sock)
					self.onConnect(client_sock)
				else: # msg from already connected client
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
	port = 12345
	PServer = Server(port)
	PServer.start()
	while True:
		sleep(10)

# %%
