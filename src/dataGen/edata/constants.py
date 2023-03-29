import os
import sys


DEBUG = sys.gettrace() is not None

WORKING_DIR = os.getcwd()
if DEBUG:
    DOCS_DIR = os.path.join(WORKING_DIR, 'docs')
else:
    DOCS_DIR = os.path.join(WORKING_DIR[:WORKING_DIR.find('src')], 'docs')
SAMPLES_DIR = os.path.join(DOCS_DIR, 'samples')

PVC_URL = 'https://www.inecnigeria.org/collections'

RES_URL = 'https://cvr.inecnigeria.org/results'

ASSM_URL = 'https://ncka74vel8.execute-api.eu-west-2.amazonaws.com/abuja-prod/elections'

SENATE = '5f129a04df41d910dcdc1d52'

REPS = '5f129a04df41d910dcdc1d53'

HOA = '5f129a04df41d910dcdc1d54'

HEADERS_REP = {
    'authority': 'ncka74vel8.execute-api.eu-west-2.amazonaws.com',
    'accept': 'application/json, text/plain, */*',
    'accept-language': 'en-GB,en;q=0.7',
    'if-none-match': 'W/"2b69-8UzjSJ/ibF1DWKy8P4i7mZ8QOBY"',
    'origin': 'https://inecelectionresults.ng',
    'referer': 'https://inecelectionresults.ng/',
    'sec-ch-ua': '"Chromium";v="110", "Not A(Brand";v="24", "Brave";v="110"',
    'sec-ch-ua-mobile': '?0',
    'sec-ch-ua-platform': '"macOS"',
    'sec-fetch-dest': 'empty',
    'sec-fetch-mode': 'cors',
    'sec-fetch-site': 'cross-site',
    'sec-gpc': '1',
    'user-agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36',
    'x-api-key': '4SXkHM7Amb1SbF4C8do6816dmbbwqPp7akRbrmcV',
    'x-api-rt': '1679016719592',
}

HEADERS_SEN = {
    'authority': 'ncka74vel8.execute-api.eu-west-2.amazonaws.com',
    'accept': 'application/json, text/plain, */*',
    'accept-language': 'en-GB,en;q=0.7',
    'if-none-match': 'W/"6e663-B1Obp6qRfXB83Qx5SKzj17iq8cE"',
    'origin': 'https://inecelectionresults.ng',
    'referer': 'https://inecelectionresults.ng/',
    'sec-ch-ua': '"Chromium";v="110", "Not A(Brand";v="24", "Brave";v="110"',
    'sec-ch-ua-mobile': '?0',
    'sec-ch-ua-platform': '"macOS"',
    'sec-fetch-dest': 'empty',
    'sec-fetch-mode': 'cors',
    'sec-fetch-site': 'cross-site',
    'sec-gpc': '1',
    'user-agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36',
    'x-api-key': '4SXkHM7Amb1SbF4C8do6816dmbbwqPp7akRbrmcV',
    'x-api-rt': '1679021650614',
}

HEADERS_HOA = {
    'authority': 'ncka74vel8.execute-api.eu-west-2.amazonaws.com',
    'accept': 'application/json, text/plain, */*',
    'accept-language': 'en-GB,en;q=0.7',
    'origin': 'https://inecelectionresults.ng',
    'referer': 'https://inecelectionresults.ng/',
    'sec-ch-ua': '"Chromium";v="110", "Not A(Brand";v="24", "Brave";v="110"',
    'sec-ch-ua-mobile': '?0',
    'sec-ch-ua-platform': '"macOS"',
    'sec-fetch-dest': 'empty',
    'sec-fetch-mode': 'cors',
    'sec-fetch-site': 'cross-site',
    'sec-gpc': '1',
    'user-agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36',
    'x-api-key': '4SXkHM7Amb1SbF4C8do6816dmbbwqPp7akRbrmcV',
    'x-api-rt': '1679024981290',
}
