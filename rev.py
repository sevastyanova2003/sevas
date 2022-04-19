import sys


def caesar(text, mode, key):
    key = int(key)
    ans = ''
    if not mode:
        key *= -1
    for x in text:
        if 'a' <= x <= 'z':
            ans += (chr((ord(x) - ord('a') + key + 26) % 26 + ord('a')))
        elif 'A' <= x <= 'Z':
            ans += (chr((ord(x) - ord('A') + key + 26) % 26 + ord('A')))            
        else:
            ans += x
    return ans

            
def vigenere(text, mode, key):
    if not mode:
        key *= -1
    ans = ''
    cnt = 0
    for x in text:
        if 'a' <= x <= 'z':
            ans += (chr((ord(x) - ord('a') + ord(key[cnt % len(key)]) - ord('a') + 26) % 26 + ord('a')))
            cnt += 1
        elif 'A' <= x <= 'Z':
            ans += (chr((ord(x) - ord('A') + ord(key[cnt % len(key)]) - ord('a') + 26) % 26 + ord('A')))
            cnt += 1        
        else:
            ans += x
    return ans

            
def vernam(text, mode, key):
    ans = ''
    if mode:
        cnt = 0
        for x in text:
            if 'a' <= x <= 'z' or 'A' <= x <= 'Z':
                ans += (str(ord(x) ^ ord(key[cnt % len(key)])))
                ans += ' '
                cnt += 1            
            else:
                ans += x
    else:
        cnt = 0
        i = 0
        while i < len(text):
            cur = 0
            while i < len(text) and (not '0' <= text[i] <= '9'):
                ans += text[i]
                i += 1
            while i < len(text) and '0' <= text[i] <= '9':
                cur *= 10
                cur += int(text[i])
                i += 1
            ans += (chr(cur ^ ord(key[cnt % len(key)])))
            i += 1
            cnt += 1
    return ans

            
def cipher(text, name, mode, key):
    ans = 'incorrect name of cipher'
    if name == 'caesar':
        ans = caesar(text, mode, key)
    elif name == 'vigenere':
        ans = vigenere(text, mode, key)
    elif name == 'vernam':
        ans = vernam(text, mode, key)
    output.write(ans)

    
def autodecipher(text):
    diff = [0] * 26
    alphabet_freq = [8.17, 1.49, 2.78, 4.25, 12.7, 2.29, 2.02, 6.09, 6.97, 0.15, 0.77,
            4.03, 2.41, 6.75, 7.51, 1.93, 0.1, 5.99, 6.33, 9.06, 2.76, 0.98, 2.36,
            0.15, 1.97, 0.07]
    for key in range(26):
        res = caesar(text, False, key)
        cnt = 0
        freq = [0] * 26
        for x in res:
            if 'a' <= x <= 'z':
                freq[ord(x) - ord('a')] += 1
                cnt += 1
            if 'A' <= x <= 'Z':
                freq[ord(x) - ord('A')] += 1
                cnt += 1            
        for i in range(26):
            diff[key] += abs(alphabet_freq[i] - freq[i] / cnt) ** 2
    ans = 0
    for i in range(26):
        if diff[i] < diff[ans]:
            ans = i
    output.write(caesar(text, False, ans))


input = open(sys.argv[1], 'r')
text = input.read()
output = open("fileout.txt", 'w')

if sys.argv[3] == 'autodecipher':
    autodecipher(text)
else:
    cipher(text, sys.argv[2], (sys.argv[3] == 'cipher'), sys.argv[4])
    
output.close()
