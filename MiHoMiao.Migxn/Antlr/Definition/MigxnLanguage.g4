parser grammar MigxnLanguage;

import MigxnStmt;

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

language
    : method;

method
    : Fun FuncName = Name LRound RRound Colon ReturnType = Name Arrow Body = statement;
