import socket, struct

multi_gruop = "224.3.29.71"
server_address = ('', 11000)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

sock.bind(server_address)
sock.settimeout(1.0)

group = socket.inet_aton(multi_gruop)
mreq = struct.pack("4sL", group, socket.INADDR_ANY)
sock.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, mreq)

while True:
    try:
        data, addr = sock.recvfrom(1024)
        print "MSG:", data
        Ret = eval(data)
        sock.sendto(str(Ret), addr)
    except socket.error, m:
        print m
sock.close()